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
        protected override GameObject Install(GameObject go, HorizontalOrVerticalLayoutElement horzOrVert, IReadOnlyLayoutContext context)
        {
            ContentSizeFitter fitter = null;
            if (context.ParseBool(horzOrVert.FitSize))
                fitter = go.AddComponent<ContentSizeFitter>();
            
            HorizontalOrVerticalLayoutGroup group = null;
            if (horzOrVert is VerticalLayoutElement vert)
            {
                group = go.AddComponent<VerticalLayoutGroup>();
                group.childControlHeight = group.childControlWidth = context.ParseBool(vert.Flex);
                if (fitter != null)
                    fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
            }
            else if (horzOrVert is HorizontalLayoutElement hor)
            {
                group = go.AddComponent<HorizontalLayoutGroup>();
                group.childControlHeight = group.childControlWidth = context.ParseBool(hor.Flex);
                
                if (fitter != null)
                    fitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
            }

            var padding = ParseUtils.ParsePadding(horzOrVert.Padding);
            group.padding = new RectOffset((int)padding.w, (int)padding.y, (int)padding.x, (int)padding.z);
            group.spacing = context.ParseFloat(horzOrVert.Spacing);
            group.childControlWidth |= context.ParseBool(horzOrVert.ExpandWidth);
            group.childForceExpandWidth = context.ParseBool(horzOrVert.ExpandWidth);
            group.childForceExpandHeight = context.ParseBool(horzOrVert.ExpandHeight);
            group.childControlHeight |= context.ParseBool(horzOrVert.ExpandHeight);
            
            if (string.IsNullOrEmpty(horzOrVert.Align))
                horzOrVert.Align = "middle";
            
            if (string.IsNullOrEmpty(horzOrVert.VertAlign))
                horzOrVert.VertAlign = "middle";

            var alignment = ParseUtils.ParseAlignment(horzOrVert.Align, horzOrVert.VertAlign);
            if (alignment.HasValue)
                group.childAlignment = alignment.Value;

            return go;
        }

        protected override void PostInstall(GameObject go, HorizontalOrVerticalLayoutElement element, IReadOnlyLayoutContext context)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(go.GetComponent<RectTransform>());
        }
    }
}