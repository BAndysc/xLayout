using System.Reflection;
using UnityEngine;
using xLayout.Definitions;

namespace xLayout.TypesConstructors
{
    public class GameObjectConstructor : TypeConstructor<GameObjectElement>
    {
        protected override GameObject Install(GameObject go, GameObjectElement element, IReadOnlyLayoutContext context)
        {
            foreach (var setter in element.Setters)
            {
                var obj = DecodePath(go, setter.Path);

                if (obj == null)
                {
                    Debug.LogError($"Cannot decode path {setter.Path} for gameobject {go}", go);
                    continue;
                }
                
                var component = obj.GetComponent(setter.Component);

                if (component == null)
                {
                    Debug.LogError($"Cannot find component `{setter.Component}` on {obj}", obj);
                    continue;
                }

                var field = component.GetType().GetField(setter.Field,
                    BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

                if (field == null)
                {
                    Debug.LogError($"Cannot find field `{setter.Field}` on component {component}", obj);
                    continue;                    
                }

                var value = setter.Value;
                
                if (field.FieldType == typeof(string))
                    field.SetValue(component, context.ParseString(value));
                else if (field.FieldType == typeof(int))
                    field.SetValue(component, context.ParseInt(value));
                else if (field.FieldType == typeof(float))
                    field.SetValue(component, context.ParseFloat(value));
                else if (field.FieldType == typeof(Vector2))
                    field.SetValue(component, context.ParseVector2(value));
                else if (field.FieldType == typeof(Vector3))
                    field.SetValue(component, context.ParseVector3(value));
                else if (field.FieldType == typeof(Vector4))
                    field.SetValue(component, context.ParseVector4(value));
                else if (field.FieldType == typeof(Color))
                    field.SetValue(component, context.ParseColor(value));
                else if (field.FieldType.IsSubclassOf(typeof(Object)))
                {
                    var asset = context.GetAsset<Object>(value);
                    field.SetValue(component, asset);                    
                }
                else
                    Debug.LogError($"Don't know how to set value of field type {field.FieldType} in {component}/{field.Name}");
            }
            return go;
        }

        private GameObject DecodePath(GameObject root, string path)
        {
            if (string.IsNullOrEmpty(path) || path == ".")
                return root;

            var parts = path.Split('/');

            var curGo = root;
            
            for (int i = 0; i < parts.Length; ++i)
            {
                var part = parts[i];

                if (part == "..")
                    curGo = curGo.transform.parent.gameObject;
                else
                    curGo = curGo.transform.Find(part)?.gameObject;

                if (curGo == null)
                    return null;
            }

            return curGo;
        }
    }
}