using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using xLayout.Definitions;

namespace xLayout.TypesConstructors
{
    public class ImageConstructor : TypeConstructor<ImageElement>
    {
        protected override GameObject Install(GameObject go, ImageElement element)
        {
            var img = go.AddComponent<Image>();
            img.color = ParseUtils.ParseColor(element.Color);
            img.sprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/" + element.Image);
            img.preserveAspect = element.PreserveAspect;

            return go;
        }
    }
}