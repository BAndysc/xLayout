using System;
using UnityEngine;
using UnityEngine.UI;
using xLayout.Definitions;

namespace xLayout.TypesConstructors
{
    public class VerticalLayoutConstructor : HorizontalOrVerticalLayoutConstructor
    {
        public override Type ConstructedType => typeof(VerticalLayoutElement);
    }
    
    public class HorizontalLayoutConstructor : HorizontalOrVerticalLayoutConstructor
    {
        public override Type ConstructedType => typeof(HorizontalLayoutElement);
    }
    
    public abstract class HorizontalOrVerticalLayoutConstructor : TypeConstructor<HorizontalOrVerticalLayoutElement>
    {
        protected override GameObject Install(GameObject go, HorizontalOrVerticalLayoutElement horzOrVert)
        {
            ContentSizeFitter fitter = null;
            if (horzOrVert.FitSize)
            {
                fitter = go.AddComponent<ContentSizeFitter>();
            }
            
            HorizontalOrVerticalLayoutGroup group = null;
            if (horzOrVert is VerticalLayoutElement vert)
            {
                group = go.AddComponent<VerticalLayoutGroup>();
                group.childControlHeight = vert.Flex.HasValue && vert.Flex.Value;
                if (fitter != null)
                    fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
            }
            else if (horzOrVert is HorizontalLayoutElement hor)
            {
                group = go.AddComponent<HorizontalLayoutGroup>();
                group.childControlWidth = horzOrVert.Flex.HasValue && horzOrVert.Flex.Value;
                
                if (fitter != null)
                    fitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
            }

            var padding = ParseUtils.ParsePadding(horzOrVert.Padding);
            group.padding = new RectOffset((int)padding.w, (int)padding.y, (int)padding.x, (int)padding.z);
            group.spacing = horzOrVert.Spacing;
            group.childControlWidth |= horzOrVert.ExpandWidth;
            group.childForceExpandWidth = horzOrVert.ExpandWidth;
            group.childForceExpandHeight = horzOrVert.ExpandHeight;
            group.childControlHeight |= horzOrVert.ExpandHeight;
            
            if (string.IsNullOrEmpty(horzOrVert.Align))
                horzOrVert.Align = "middle";
            
            if (string.IsNullOrEmpty(horzOrVert.VertAlign))
                horzOrVert.VertAlign = "middle";

            var alignment = ParseUtils.ParseAlignment(horzOrVert.Align, horzOrVert.VertAlign);
            if (alignment.HasValue)
                group.childAlignment = alignment.Value;

            return go;
        }

        protected override void PostInstall(GameObject go, HorizontalOrVerticalLayoutElement element)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(go.GetComponent<RectTransform>());
        }
    }
}