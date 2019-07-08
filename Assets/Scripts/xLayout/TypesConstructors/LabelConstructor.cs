using TMPro;
using UnityEngine;
using UnityEngine.UI;
using xLayout.Definitions;

namespace xLayout.TypesConstructors
{
    public class LabelConstructor : TypeConstructor<LabelElement>
    {
        protected override GameObject Install(GameObject go, LabelElement element)
        {
            var rect = go.GetComponent<RectTransform>();
            var oldOffsetMin = rect.offsetMin;
            var oldOffsetMax = rect.offsetMax;
            
            var textMesh = go.AddComponent<TextMeshProUGUI>();
            textMesh.text = element.Text;
            textMesh.fontSize = element.FontSize;

            if (string.IsNullOrEmpty(element.Align))
                element.Align = "middle";
            
            if (string.IsNullOrEmpty(element.VertAlign))
                element.VertAlign = "middle";

            var alignment = ParseUtils.ParseTextMeshProAlignment(element.Align, element.VertAlign);
            if (alignment.HasValue)
                textMesh.alignment = alignment.Value;

            textMesh.color = ParseUtils.ParseColor(element.Color);

            if (!string.IsNullOrEmpty(element.FitSize))
            {
                var horiz = element.FitSize == "horizontal" || element.FitSize == "both";
                var vert = element.FitSize == "vertical" || element.FitSize == "both";
                var contentFitter = go.AddComponent<ContentSizeFitter>();
                contentFitter.verticalFit = vert ? ContentSizeFitter.FitMode.PreferredSize : ContentSizeFitter.FitMode.Unconstrained;
                contentFitter.horizontalFit = horiz ? ContentSizeFitter.FitMode.PreferredSize : ContentSizeFitter.FitMode.Unconstrained;
            }
            
            rect.offsetMin = oldOffsetMin;
            rect.offsetMax = oldOffsetMax;
            
            return go;
        }
    }
}