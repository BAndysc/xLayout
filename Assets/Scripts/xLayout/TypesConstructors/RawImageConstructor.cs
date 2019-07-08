using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using xLayout.Definitions;

namespace xLayout.TypesConstructors
{
    public class RawImageConstructor : TypeConstructor<RawImageElement>
    {
        protected override GameObject Install(GameObject go, RawImageElement element)
        {
            var img = go.AddComponent<RawImage>();

            img.color = ParseUtils.ParseColor(element.Color);
            img.texture = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/" + element.Image);

            return go;
        }
    }
}