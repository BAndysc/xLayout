using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using xLayout.Definitions;

namespace xLayout.TypesConstructors
{
    [TypeConstructor]
    public class ImageConstructor : TypeConstructor<ImageElement>
    {
        protected override GameObject Install(GameObject go, ImageElement element, IReadOnlyLayoutContext context)
        {
            var img = go.AddComponent<Image>();
            img.color = context.ParseColor(element.Color);
            img.sprite = context.GetAsset<Sprite>(element.Image);
            img.preserveAspect = context.ParseBool(element.PreserveAspect);
            img.material = context.GetAsset<Material>(element.Material);

            return go;
        }
    }
}