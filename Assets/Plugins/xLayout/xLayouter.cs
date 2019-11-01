using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml.Serialization;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using xLayout.Animations;
using xLayout.Definitions;
using xLayout.TypesConstructors;
using Object = UnityEngine.Object;

namespace xLayout
{
    public static class xLayouter
    {
        private static XmlSerializer serializer;
        private static XmlSerializer resourceDeserializer;
        
        static xLayouter()
        {
            serializer = new XmlSerializer(typeof(CanvasDefinition));
            resourceDeserializer = new XmlSerializer(typeof(ResourcesDefinition));

            LogToUnitConsole(serializer);
            LogToUnitConsole(resourceDeserializer);
        }

        private static void LogToUnitConsole(XmlSerializer serializer)
        {
            serializer.UnknownNode += (o, e) => Debug.LogError("Error UNKNOWN NODE, " + e.Name + $" {e.LocalName} {e.NodeType}  at {e.LineNumber}");
            serializer.UnknownElement += (o, e) => Debug.LogError("Error UnknownElement, " + e.ToString());
            serializer.UnknownAttribute += (o, e) => Debug.LogError("Error UnknownAttribute, " + e.ToString());
            serializer.UnreferencedObject += (o, e) => Debug.LogError("Error UnreferencedObject, " + e.ToString());
        }
        
        public static void BuildLayoutFromXML(GameObject parent, string xmlFile, out HashSet<string> referencedResources)
        {
            referencedResources = new HashSet<string>();
            
            StreamReader reader = new StreamReader(xmlFile);
            var canvas = (CanvasDefinition)serializer.Deserialize(reader);

            LayoutContext context = new LayoutContext();
            ParseResources(context, canvas.Resources, Path.GetDirectoryName(xmlFile), referencedResources);
            
            KillAllChildren(parent);

            BuildLayout(parent, canvas.Elements, context);
            reader.Close();            
        }

        private static LayoutContext BuildContextFromResources(string xmlFile, HashSet<string> referencedResources)
        {
            StreamReader reader = new StreamReader(xmlFile);
            var resources = (ResourcesDefinition)resourceDeserializer.Deserialize(reader);

            LayoutContext context = new LayoutContext();
            ParseResources(context, resources.Resources, Path.GetDirectoryName(xmlFile), referencedResources);

            return context;
        }

        private static void ParseResources(LayoutContext context, List<ResourceElement> canvasResources,
            string workingDir, HashSet<string> referencedResources)
        {
            foreach (var resource in canvasResources)
            {
                if (resource is ResourceVariableElement variableElement)
                    context.AddVariable(resource.Name, variableElement.Value);
                else if (resource is ResourceAssetElement assetElement)
                    context.AddAsset(resource.Name, assetElement.Path);
                else if (resource is ResourceIncludeElement includeElement)
                {
                    referencedResources.Add(includeElement.Path);
                    
                    var path = workingDir + "/" + includeElement.Path;
                    if (!File.Exists(path))
                    {
                        Debug.LogError("Trying to include resource: " + path + " but the file doesn't exist.");
                        continue;
                    }

                    var resourceContext = BuildContextFromResources(path, referencedResources);
                    context.MergeResource(resourceContext);
                }
                else if (resource is ResourcePrefabElement prefabElement)
                {
                    if (prefabElement.Content.Elements.Count != 1)
                    {
                        Debug.Assert(false, $"Prefab should have exactly one child (prefab `{prefabElement.Name}`)");
                        continue;
                    }

                    if (!(prefabElement.Content.Elements[0] is RectTransformElement))
                    {
                        Debug.Assert(false, "Prefab first child cannot be scroll or layout");
                        continue;
                    }

                    context.AddPrefab(prefabElement.Name, prefabElement);

                }
                else
                    throw new NotImplementedException();
            }
        }

        private static void KillAllChildren(GameObject gameObject)
        {
            while (gameObject.transform.childCount > 0)
                Object.DestroyImmediate(gameObject.transform.GetChild(0).gameObject);
        }
        
        private static GameObject CreateElement(RectTransformElement element, Transform parent, IReadOnlyLayoutContext context, out GameObject newParent)
        {
            GameObject go = null;
            List<RectTransformElement> originalElements = null;
            
            while (element is PrefabElement prefab)
            {
                var prefabElement = context.GetPrefab(prefab.Prefab);
                if (prefabElement != null)
                {
                    if (originalElements == null)
                        originalElements = new List<RectTransformElement>();
                    
                    originalElements.Add(element);
                    
                    element = prefabElement.Content.Elements[0] as RectTransformElement;
                }
            }
            
            if (element is GameObjectElement gameobject)
            {
                go = PrefabUtility.InstantiatePrefab(context.GetAsset<GameObject>(gameobject.Path)) as GameObject;
                go.name = element.Name;
            }
            else
            {
                go = new GameObject(element.Name);                
            }
            go.transform.parent = parent;
            go.transform.localScale = Vector3.one;

            go.AddComponent<ExternalLayoutWarning>();

            newParent = go;
            
            if (element.Active == "false")
                go.SetActive(false);

            var rect = go.GetComponent<RectTransform>();
    
            if (rect == null)
                rect = go.AddComponent<RectTransform>();
            
            if (!string.IsNullOrEmpty(element.Padding))
            {
                var padding = context.ParsePadding(element.Padding);
                GameObject padder = new GameObject();
                padder.name = $"Padding ({go.name})";
                padder.transform.parent = rect;
                padder.transform.localScale = Vector3.one;
                rect = padder.AddComponent<RectTransform>();
    
                rect.anchorMin = Vector2.zero;
                rect.anchorMax = Vector2.one;
    
                rect.offsetMin = new Vector2(padding.w, padding.z);
                rect.offsetMax = new Vector2(-padding.y, -padding.x);

                newParent = padder;
            }

            ApplyTransformSettings(element, parent, context, go);

            if (originalElements != null)
            {
                for (int i = originalElements.Count - 1; i >= 0; --i)
                {
                    var originalElement = originalElements[i];
                    ApplyTransformSettings(originalElement, parent, context, go);
                    if (!string.IsNullOrEmpty(originalElement.Name))
                        go.name = originalElement.Name;                    
                }

            }
            return go;
        }

        private static void ApplyTransformSettings(RectTransformElement element, Transform parent,
            IReadOnlyLayoutContext context, GameObject go)
        {
            var rect = go.GetComponent<RectTransform>();

            var dock = context.ParseString(element.Dock);
            if (dock == "fill")
                element.AnchorX = element.AnchorY = "(0, 1)";
            else if (dock == "left")
            {
                element.AnchorX = "(0, 0)";
                element.AnchorY = "(0, 1)";
                element.Pivot = "(0, 0.5)";
            }
            else if (dock == "right")
            {
                element.AnchorX = "(1, 1)";
                element.AnchorY = "(0, 1)";
                element.Pivot = "(1, 0.5)";
            }
            else if (dock == "top")
            {
                element.AnchorX = "(0, 1)";
                element.AnchorY = "(1, 1)";
                element.Pivot = "(0.5, 1)";
            }
            else if (dock == "bottom")
            {
                element.AnchorX = "(0, 1)";
                element.AnchorY = "(0, 0)";
                element.Pivot = "(0.5, 0)";
            }
             
            if (!string.IsNullOrEmpty(element.AnchorX))
            {
                rect.anchorMin = new Vector2(context.ParseVector2(element.AnchorX).x, rect.anchorMin.y);
                rect.anchorMax = new Vector2(context.ParseVector2(element.AnchorX).y, rect.anchorMax.y);
            }

            if (!string.IsNullOrEmpty(element.AnchorY))
            {
                rect.anchorMin = new Vector2(rect.anchorMin.x, context.ParseVector2(element.AnchorY).x);
                rect.anchorMax = new Vector2(rect.anchorMax.x, context.ParseVector2(element.AnchorY).y);
            }

            if (!string.IsNullOrEmpty(element.Pivot))
                rect.pivot = context.ParseVector2(element.Pivot);

            rect.offsetMax = new Vector2(0, 0);
            rect.offsetMin = Vector2.zero;
            
            if (!string.IsNullOrEmpty(element.Top))
            {
                if (Mathf.Abs(rect.anchorMin.y - rect.anchorMax.y) > 0.01f)
                    rect.offsetMax = new Vector2(rect.offsetMax.x, -context.ParseFloat(element.Top));
                else
                    Debug.LogError("Property Top cannot work if AnchorY is single value");
            }

            if (!string.IsNullOrEmpty(element.Bottom))
            {
                if (Mathf.Abs(rect.anchorMin.y - rect.anchorMax.y) > 0.01f)
                    rect.offsetMin = new Vector2(rect.offsetMin.x, context.ParseFloat(element.Bottom));
                else
                    Debug.LogError("Property Bottom cannot work if AnchorY is single value");
            }


            if (!string.IsNullOrEmpty(element.Left))
            {
                if (Mathf.Abs(rect.anchorMin.x - rect.anchorMax.x) > 0.01f)
                    rect.offsetMin = new Vector2(context.ParseFloat(element.Left), rect.offsetMin.y);
                else
                    Debug.LogError("Property Left cannot work if AnchorX is single value");
            }

            if (!string.IsNullOrEmpty(element.Right))
            {
                if (Mathf.Abs(rect.anchorMin.x - rect.anchorMax.x) > 0.01f)
                    rect.offsetMax = new Vector2(-context.ParseFloat(element.Right), rect.offsetMax.y);
                else
                    Debug.LogError("Property Right cannot work if AnchorX is single value");
            }

            if (!string.IsNullOrEmpty(element.Width))
            {
                if (parent.gameObject.GetComponent<HorizontalOrVerticalLayoutGroup>()?.childControlWidth ?? false)
                {
                    var layoutElement = go.AddComponent<LayoutElement>();
                    if (element.Width == "fill")
                        layoutElement.flexibleWidth = 1;
                    else
                        layoutElement.minWidth = context.ParseFloat(element.Width);
                }
                else
                    rect.sizeDelta = new Vector2(context.ParseFloat(element.Width), rect.sizeDelta.y);
            }

            if (!string.IsNullOrEmpty(element.Height))
            {
                if (parent.gameObject.GetComponent<HorizontalOrVerticalLayoutGroup>()?.childControlHeight ?? false)
                {
                    var layoutElement = go.AddComponent<LayoutElement>();
                    if (element.Height == "fill")
                        layoutElement.flexibleHeight = 1;
                    else
                        layoutElement.minHeight = context.ParseFloat(element.Height);
                }
                else
                    rect.sizeDelta = new Vector2(rect.sizeDelta.x, context.ParseFloat(element.Height));
            }


            if (!string.IsNullOrEmpty(element.Offset))
            {
                var offset = context.ParseVector2(element.Offset);
                rect.anchoredPosition += new Vector2(offset.x, -offset.y);
            }
        }

        public static void BuildLayout(GameObject parent, IEnumerable<BaseElement> children, IReadOnlyLayoutContext context)
        {
            foreach (var child in children)
            {
                Deserialize(child, parent, context);
            }
        }
        
        private static Dictionary<string, GameObject> Deserialize(BaseElement element, GameObject gameObject, IReadOnlyLayoutContext context)
        {
            GameObject parent = gameObject;
            Dictionary<string, GameObject> byKeys = new Dictionary<string, GameObject>();
            
            if (element is RectTransformElement rte)
            {
                gameObject = CreateElement(rte, gameObject.transform, context, out parent);
            }
            while (element is PrefabElement prefab)
            {
                var prefabElement = context.GetPrefab(prefab.Prefab);
                if (prefabElement != null)
                {
                    if (!string.IsNullOrEmpty(prefab.Key))
                        byKeys[prefab.Key] = gameObject;
                    
                    if ((prefabElement.Properties?.Count ?? 0) > 0 || 
                        (prefab.Properties?.Count ?? 0) > 0 || 
                        !string.IsNullOrEmpty(prefab.Property))
                    {
                        var newContext = new LayoutContext();
                        newContext.MergeResource((LayoutContext)context); // haaack :S

                        if (prefabElement.Properties != null)
                        {
                            foreach (var property in prefabElement.Properties)
                                if (property.HasDefault)
                                    newContext.AddProperty(property.Name, property.Default);
                        }

                        if (prefab.Properties != null)
                        {
                            foreach (var property in prefab.Properties)
                                newContext.AddProperty(property.Name, property.Value);                    
                        }

                        if (!string.IsNullOrEmpty(prefab.Property))
                        {
                            var colon = prefab.Property.IndexOf(":");
                            if (colon == -1)
                            {
                                Debug.LogError("Attribute Property in Prefab element is expected to match [a-zA-Z0-9_]+:.*");
                            }
                            else
                            {
                                var prop = prefab.Property.Substring(0, colon);
                                var value = prefab.Property.Substring(colon + 1);
                                newContext.AddProperty(prop, value);
                            }
                        }

                        context = newContext;
                    }

                    element = prefabElement.Content.Elements[0];
                }
            }

            var oldParent = parent;
            var constructor = Constructors.GetConstructor(element.GetType());
            parent = constructor.Install(gameObject, element, context);

            if (parent != gameObject)
                oldParent = parent;
            
            foreach (var subElement in element.Elements)
            {
                var keys = Deserialize(subElement, oldParent, context);
                if (!(element is PrefabElement))
                {
                    foreach (var key in keys)
                        byKeys[key.Key] = key.Value;   
                }
            }
    
            if (element is RectTransformElement rte_)
            {
                InstallComponents(rte_, gameObject, byKeys, context);
                InstallAnimations(rte_, gameObject, context);
            }
            
            constructor.PostInstall(gameObject, element, context);
            
            return byKeys;
        }

        private static void InstallAnimations(RectTransformElement rte, GameObject gameObject, IReadOnlyLayoutContext context)
        {
            foreach (var anim in rte.Animations)
            {
                UIAnimation animation;
                if (anim is CanvasAlphaAnimationElement canvasAlpha)
                {
                    CanvasGroup cg = gameObject.EnsureComponent<CanvasGroup>();
                    var alphaAnimation = gameObject.AddComponent<CanvasAlphaAnimation>();
                    animation = alphaAnimation;
                    alphaAnimation.Setup(cg, context.ParseFloat(canvasAlpha.DestValue));
                }
                else if (anim is ScaleAnimationElement scale)
                {
                    var scaleAnimation = gameObject.AddComponent<UIScaleAnimation>();
                    animation = scaleAnimation;
                    scaleAnimation.Setup(gameObject.GetComponent<RectTransform>(), context.ParseVector3(scale.DestValue), context.ParseFloat(scale.Speed, 1));
                }
                else
                    throw new Exception();

                foreach (var trigger in anim.Triggers)
                {
                    UITrigger triggerComponent;

                    if (trigger is OnPointerEnterTriggerElement)
                        triggerComponent = gameObject.AddComponent<OnPointerEnterPlayAnimation>();
                    else if (trigger is OnPointerExitTriggerElement)
                        triggerComponent = gameObject.AddComponent<OnPointerExitPlayAnimation>();
                    else if (trigger is OnPointerDownTriggerElement)
                        triggerComponent = gameObject.AddComponent<OnPointerDownPlayAnimation>();
                    else if (trigger is OnPointerUpTriggerElement)
                        triggerComponent = gameObject.AddComponent<OnPointerUpPlayAnimation>();
                    else
                        throw new Exception();

                    triggerComponent.conditions = ParseConditions(trigger.Conditions, gameObject, context);
                    
                    triggerComponent.animation = animation;
                }
            }
        }

        private static UICondition[] ParseConditions(List<ConditionElement> conditionList, GameObject go,
            IReadOnlyLayoutContext context)
        {
            var conditions = new UICondition[conditionList.Count];

            for (int i = 0; i < conditionList.Count; ++i)
            {
                UICondition uiCondition = null;
                var condition = conditionList[i];

                if (condition is ConditionPointerOverElement)
                    uiCondition = go.AddComponent<PointerIsOverCondition>();
                else if (condition is ConditionNotElement notElement)
                {
                    var not = go.AddComponent<InverseCondition>();
                    not.originals = ParseConditions(notElement.Conditions, go, context);
                    Debug.Assert(not.originals.Length == 1);
                    uiCondition = not;
                }                
                
                conditions[i] = uiCondition;
            }

            return conditions;
        }

        private static void InstallComponents(RectTransformElement element, GameObject gameObject, Dictionary<string,GameObject> byKeys, IReadOnlyLayoutContext context)
        {
            if (!string.IsNullOrEmpty(element.Key))
            {
                if (byKeys.ContainsKey(element.Key))
                    Debug.LogError($"Duplicate key: {element.Key}. Key is supposed to be unique in whole XML, thus it is different than name");
                byKeys[element.Key] = gameObject;
            }
            byKeys["{this}"] = gameObject;
    
            if (element.Components != null)
            {
                foreach (var componentElement in element.Components)
                {
                    var type = ComponentInstaller.GetMonoBehaviourTypeOrLogError(componentElement.ComponentType);
                    if (type == null)
                        continue;
                    
                    var component = gameObject.AddComponent(type);
    
                    if (componentElement.Bindings != null)
                        InstallBindings(byKeys, componentElement, type, component, context);
                }
            }
        }
    
        private static void InstallBindings(Dictionary<string, GameObject> byKeys, ComponentElement element, System.Type monoBehaviourType, Component component, IReadOnlyLayoutContext context)
        {
            foreach (var componentBinding in element.Bindings)
            {
                PropertyInfo property = null;
                var field = monoBehaviourType.GetField(componentBinding.Field,
                    BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);

                Action<object> setValue = v => field.SetValue(component, v);
                
                if (field == null)
                {
                    property = monoBehaviourType.GetProperty(componentBinding.Field,
                        BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);

                    if (property == null)
                    {
                        Debug.LogError(
                            $"Component {monoBehaviourType.FullName} doesn't have field {componentBinding.Field}");
                        continue;
                    }
                    
                    setValue = v => property.SetValue(component, v);
                }

                string fieldName = field?.Name ?? property.Name;
                Type fieldType = field?.FieldType ?? property.PropertyType;

                if (componentBinding is BindingElement binding)
                {
                    if (!byKeys.TryGetValue(binding.Source, out var source))
                    {
                        Debug.LogError($"Trying to bind object of key {binding.Source} but not found");
                        continue;
                    }
    
                    if (field.FieldType.IsSubclassOf(typeof(Component)))
                    {
                        var sourceComponent = source.GetComponent((System.Type)field.FieldType);
    
                        if (sourceComponent == null)
                            Debug.LogError($"Component {field.FieldType} not found on {source}", source);
                        else
                            setValue(sourceComponent);
                    }
                    else if (field.FieldType == typeof(GameObject))
                    {
                        setValue(source);
                    }
                    else
                    {
                        Debug.LogError($"Cannot bind type {field.FieldType}");
                    }   
                }
                else if (componentBinding is ComponentSetterElement setter)
                    ReflectionUtils.ReflectionSetComponentField(context, fieldName, fieldType, setValue, component, setter.Value);
            }
        }
    }
}