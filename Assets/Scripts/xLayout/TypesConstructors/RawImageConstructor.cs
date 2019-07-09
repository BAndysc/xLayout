using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using xLayout.Definitions;

namespace xLayout.TypesConstructors
{
    public class RawImageConstructor : TypeConstructor<RawImageElement>
    {
        protected override GameObject Install(GameObject go, RawImageElement element, IReadOnlyLayoutContext context)
        {
            var img = go.AddComponent<RawImage>();

            img.color = context.ParseColor(element.Color);
            img.texture = context.GetAsset<Texture2D>(element.Image);
            img.material = context.GetAsset<Material>(element.Material);

            return go;
        }
    }
}