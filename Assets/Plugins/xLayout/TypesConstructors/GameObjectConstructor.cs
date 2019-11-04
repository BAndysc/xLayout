using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using xLayout.Definitions;

namespace xLayout.TypesConstructors
{
    [TypeConstructor]
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
                ReflectionUtils.ReflectionSetComponentField(context, field.Name, field.FieldType, v => field.SetValue(component, v), component, setter.Value);
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