using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace xLayout
{
    public static class ComponentInstaller
    {
        private static Dictionary<string, List<Type>> byNameTypes;

        private static Dictionary<string, Type> byNameUniqueType;
        
        static ComponentInstaller()
        {
            var monoBehaviours = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(x =>
                {
                    try
                    {
                        return x.GetTypes();
                    }
                    catch (Exception e)
                    {
                        return Enumerable.Empty<Type>();
                    }
                })
                .Where(t => !t.IsAbstract && t.IsSubclassOf(typeof(Component)));

            byNameUniqueType = new Dictionary<string, Type>();
            byNameTypes = new Dictionary<string, List<Type>>();
            
            foreach (var monoBehaviourType in monoBehaviours)
            {
                var key = monoBehaviourType.Name;

                if (byNameTypes.TryGetValue(key, out var list))
                    list.Add(monoBehaviourType);
                else
                    byNameTypes[key] = new List<Type>() {monoBehaviourType};
            }

            foreach (var types in byNameTypes)
            {
                if (types.Value.Count == 1)
                {
                    var type = types.Value[0];
                    byNameUniqueType.Add(type.Name, type);
                }
                
                foreach (var type in types.Value)
                {
                    if (type.FullName == type.Name)
                        byNameUniqueType.Add("." + type.Name, type);
                    else
                        byNameUniqueType.Add(type.FullName, type);
                }
            }
        }

        public static Type GetMonoBehaviourTypeOrLogError(string name)
        {
            if (byNameUniqueType.TryGetValue(name, out var type))
                return type;

            if (byNameTypes.ContainsKey(name))
            {
                Debug.LogError($"There are many MonoBehaviours with name {name}: {string.Join(", ", byNameTypes[name].Select(t => t.FullName))}. You have to specify Full class Name");
            }
            else
                Debug.LogError($"No MonoBehaviour type matching {name}");

            return null;
        }
    }
}