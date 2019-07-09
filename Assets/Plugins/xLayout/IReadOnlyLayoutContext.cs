using UnityEngine;
using xLayout.Definitions;

namespace xLayout
{
    public interface IReadOnlyLayoutContext
    {
        string ParseString(string value);
        bool ParseBool(string value, bool? @default = null);
        int ParseInt(string value);
        float ParseFloat(string value);
        Color ParseColor(string value);
        Vector4 ParsePadding(string value);
        Vector2 ParseVector2(string value);
        Vector3 ParseVector3(string value);
        Vector4 ParseVector4(string value);
        T GetAsset<T>(string value)  where T : Object;
        ResourcePrefabElement GetPrefab(string prefabName);
    }
}