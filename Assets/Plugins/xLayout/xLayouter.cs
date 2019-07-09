using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml.Serialization;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using xLayout.Definitions;
using xLayout.TypesConstructors;
using Object = UnityEngine.Object;

namespace xLayout
{
    public static class xLayouter
    {
        private static XmlSerializer serializer;
        
        static xLayouter()
        {
            serializer = new XmlSerializer(typeof(CanvasDefinition));

            serializer.UnknownNode += (o, e) => Debug.LogError("Error UNKNOWN NODE, " + e.Name + $" {e.LocalName} {e.NodeType}  at {e.LineNumber}");
            serializer.UnknownElement += (o, e) => Debug.LogError("Error UnknownElement, " + e.ToString());
            serializer.UnknownAttribute += (o, e) => Debug.LogError("Error UnknownAttribute, " + e.ToString());
            serializer.UnreferencedObject += (o, e) => Debug.LogError("Error UnreferencedObject, " + e.ToString());
        }
        
        public static void BuildLayoutFromXML(GameObject parent, string xmlFile)
        {
            StreamReader reader = new StreamReader(xmlFile);
            var canvas = (CanvasDefinition)serializer.Deserialize(reader);

            LayoutContext context = new LayoutContext();
            ParseResources(context, canvas.Resources);
            
            KillAllChildren(parent);

            BuildLayout(parent, canvas.Elements, context);
            reader.Close();            
        }

        private static void ParseResources(LayoutContext context, List<ResourceElement> canvasResources)
        {
            foreach (var resource in canvasResources)
            {
                if (resource is ResourceVariableElement variableElement)
                    context.AddVariable(resource.Name, variableElement.Value);
                else if (resource is ResourceAssetElement assetElement)
                    context.AddAsset(resource.Name, assetElement.Path);
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
            if (element is PrefabElement prefab)
            {
                go = PrefabUtility.InstantiatePrefab(context.GetAsset<GameObject>(prefab.Path)) as GameObject;
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

            RectTransform childParent = go.GetComponent<RectTransform>();
            if (childParent == null)
                childParent = go.AddComponent<RectTransform>();
    
            if (!string.IsNullOrEmpty(element.Padding))
            {
                var padding = context.ParsePadding(element.Padding);
                GameObject padder = new GameObject();
                padder.name = $"Padding ({go.name})";
                padder.transform.parent = childParent;
                padder.transform.localScale = Vector3.one;
                childParent = padder.AddComponent<RectTransform>();
    
                childParent.anchorMin = Vector2.zero;
                childParent.anchorMax = Vector2.one;
    
                childParent.offsetMin = new Vector2(padding.w, padding.z);
                childParent.offsetMax = new Vector2(-padding.y, -padding.x);

                newParent = padder;
            }
            
            var rect = go.GetComponent<RectTransform>();
    
            if (rect == null)
                rect = go.AddComponent<RectTransform>();

            if (context.ParseBool(element.Dock))
            {
                element.AnchorX = element.AnchorY = "(0, 1)";
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
                        layoutElement.flexibleHeight= 1;
                    else
                        layoutElement.minHeight = context.ParseFloat(element.Height);
                }
                else
                    rect.sizeDelta = new Vector2(rect.sizeDelta.x, context.ParseFloat(element.Height));
            }
    
            return go;
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
            if (element is RectTransformElement rte)
            {
                gameObject = CreateElement(rte, gameObject.transform, context, out parent);
            }

            var oldParent = parent;
            var constructor = Constructors.GetConstructor(element.GetType());
            parent = constructor.Install(gameObject, element, context);

            if (parent != gameObject)
                oldParent = parent;
            
            Dictionary<string, GameObject> byKeys = new Dictionary<string, GameObject>();
    
            foreach (var subElement in element.Elements)
            {
                var keys = Deserialize(subElement, oldParent, context);
                foreach (var key in keys)
                    byKeys[key.Key] = key.Value;
            }
    
            if (element is RectTransformElement rte_)
            {
                InstallComponents(rte_, gameObject, byKeys);
            }
            
            constructor.PostInstall(gameObject, element, context);
            
            return byKeys;
        }
    
        private static void InstallComponents(RectTransformElement element, GameObject gameObject, Dictionary<string,GameObject> byKeys)
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
                        InstallBindings(byKeys, componentElement, type, component);
                }
            }
        }
    
        private static void InstallBindings(Dictionary<string, GameObject> byKeys, ComponentElement element, System.Type monoBehaviourType, Component component)
        {
            foreach (var componentBinding in element.Bindings)
            {
                if (!byKeys.TryGetValue(componentBinding.Source, out var source))
                {
                    Debug.LogError($"Trying to bind object of key {componentBinding.Source} but not found");
                    continue;
                }
    
                var field = monoBehaviourType.GetField(componentBinding.Field,
                    BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);
    
                if (field == null)
                {
                    Debug.LogError($"Component {monoBehaviourType.FullName} doesn't have field {componentBinding.Field}");
                    continue;
                }
    
                if (field.FieldType.IsSubclassOf(typeof(Component)))
                {
                    var sourceComponent = source.GetComponent((System.Type)field.FieldType);
    
                    if (sourceComponent == null)
                        Debug.LogError($"Component {field.FieldType} not found on {source}", source);
                    else
                        field.SetValue(component, sourceComponent);
                }
                else if (field.FieldType == typeof(GameObject))
                {
                    field.SetValue(component, source);
                }
                else
                {
                    Debug.LogError($"Cannot bind type {field.FieldType}");
                }
            }
        }
    }
}