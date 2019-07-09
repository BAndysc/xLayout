using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using xLayout.Definitions;
using Object = UnityEngine.Object;

namespace xLayout
{
    public class LayoutContext : IReadOnlyLayoutContext
    {
        static LayoutContext()
        {
            System.Globalization.CultureInfo customCulture = (System.Globalization.CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
            customCulture.NumberFormat.NumberDecimalSeparator = ".";

            System.Threading.Thread.CurrentThread.CurrentCulture = customCulture;
        }
        
        private Dictionary<string, string> variables = new Dictionary<string, string>();
        private Dictionary<string, string> assets = new Dictionary<string, string>();
        private Dictionary<string, ResourcePrefabElement> prefabs = new Dictionary<string, ResourcePrefabElement>();

        public void AddVariable(string key, string variable)
        {
            if (variables.ContainsKey(key))
                Debug.LogError($"Variable '{key}' is already defined. It has to be unique");

            if (assets.ContainsKey(key))
                Debug.LogError($"'{key}' is already defined AS asset. It has to be unique");

            variables[key] = variable;
        }
        
        public void AddAsset(string key, string variable)
        {
            if (assets.ContainsKey(key))
                Debug.LogError($"Asset '{key}' is already defined. It has to be unique");

            if (variables.ContainsKey(key))
                Debug.LogError($"'{key}' is already defined AS variable. It has to be unique");
            
            assets[key] = variable;
        }
        
        private bool IsResource(string value, out string resourceContent)
        {
            resourceContent = null;

            if (value == null)
                return false;
            
            value = value.Trim();

            if (!value.StartsWith("{") || !value.EndsWith("}"))
                return false;
                    
            var newStr = value.Substring(1, value.Length - 2).Trim();

            if (!newStr.ToLower().StartsWith("resource"))
                return false;

            resourceContent = newStr.Substring("resource".Length).Trim();

            return true;
        }
        
        private string DecodeString(string value)
        {
            if (!IsResource(value, out var resourceContent))
                return value;

            if (variables.TryGetValue(resourceContent, out var variable))
                return variable;

            if (assets.TryGetValue(resourceContent, out var asset))
                return asset;
            
            Debug.LogError($"Resource '{resourceContent}' ({value}) not found");
            return string.Empty;
        }
        
        public string ParseString(string value)
        {
            return DecodeString(value);
        }
        
        public bool ParseBool(string value, bool? @default = null)
        {
            value = DecodeString(value);

            if (string.IsNullOrEmpty(value))
                return false;
            
            var isTrue = value.Equals("true", StringComparison.InvariantCultureIgnoreCase);
            var isFalse = value.Equals("false", StringComparison.InvariantCultureIgnoreCase);

            if (!isTrue && !isFalse)
            {
                if (@default.HasValue)
                    throw new Exception($"Cannot parse bool: {value} (and it has no default. Aborting");
                return @default.Value;
            }

            return isTrue;
        }

        public int ParseInt(string value)
        {
            value = DecodeString(value);
            
            if (string.IsNullOrEmpty(value))
                return 0;
            
            if (!int.TryParse(ParseString(value), out var integer))
                throw new Exception($"Value: '{value}' is supposed to be an integer");
            return integer;
        }
        
        public float ParseFloat(string value)
        {
            value = DecodeString(value);
            
            if (string.IsNullOrEmpty(value))
                return 0;
            
            if (!float.TryParse(ParseString(value), out var leFloat))
                throw new Exception($"Value: '{value}' is supposed to be an integer");
            return leFloat;
        }

        public Color ParseColor(string value)
        {
            value = DecodeString(value);

            return ParseUtils.ParseColor(value);
        }

        public Vector4 ParsePadding(string value)
        {
            value = DecodeString(value);

            return ParseUtils.ParsePadding(value);
        }

        public Vector2 ParseVector2(string value)
        {
            value = DecodeString(value);

            return ParseUtils.ParseVector2(value);
        }
        
        public Vector3 ParseVector3(string value)
        {
            value = DecodeString(value);

            return ParseUtils.ParseVector3(value);
        }
        
        public Vector4 ParseVector4(string value)
        {
            value = DecodeString(value);

            return ParseUtils.ParseVector4(value);
        }

        public T GetAsset<T>(string value) where T : Object
        {
            value = DecodeString(value);

            if (string.IsNullOrEmpty(value))
                return null;

            return AssetDatabase.LoadAssetAtPath<T>("Assets/" + value);
        }

        public ResourcePrefabElement GetPrefab(string prefabName)
        {
            if (prefabs.TryGetValue(prefabName, out var element))
                return element;

            Debug.LogError($"Prefab named `{prefabName}` not found!");
            
            return null;
        }

        public void MergeResource(LayoutContext otherContext)
        {
            foreach (var asset in otherContext.assets)
            {
                if (!assets.ContainsKey(asset.Key))
                    assets[asset.Key] = asset.Value;
            }
            
            foreach (var variable in otherContext.variables)
            {
                if (!variables.ContainsKey(variable.Key))
                    variables[variable.Key] = variable.Value;
            }
            
            foreach (var prefab in otherContext.prefabs)
            {
                if (!prefabs.ContainsKey(prefab.Key))
                    prefabs[prefab.Key] = prefab.Value;
            }
        }

        public void AddPrefab(string name, ResourcePrefabElement baseElement)
        {
            prefabs[name] = baseElement;
        }

        public void AddProperty(string propertyName, string propertyValue)
        {
            variables[propertyName] = ParseString(propertyValue);
        }
    }
}