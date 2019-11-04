using System;
using UnityEngine;
using UnityEngine.UI;
using xLayout.Definitions;

namespace xLayout.TypesConstructors
{
    [TypeConstructor]
    public class GridLayoutConstructor : TypeConstructor<GridLayoutElement>
    {
        protected override GameObject Install(GameObject go, GridLayoutElement element, IReadOnlyLayoutContext context)
        {
            var gridLayout = go.AddComponent<GridLayoutGroup>();

            if (!string.IsNullOrEmpty(element.Spacing))
                gridLayout.spacing = context.ParseVector2(element.Spacing);

            if (!string.IsNullOrEmpty(element.CellSize))
                gridLayout.cellSize = context.ParseVector2(element.CellSize);
            else
                gridLayout.cellSize = Vector2.one * 250;

            if (!string.IsNullOrEmpty(element.Padding))
            {
                var padding = context.ParsePadding(element.Padding);

                gridLayout.padding.top = (int)padding.x;
                gridLayout.padding.right = (int)padding.y;
                gridLayout.padding.bottom = (int)padding.z;
                gridLayout.padding.left = (int)padding.w;
            }

            if (string.IsNullOrEmpty(element.Align))
                element.Align = "left";
                
            if (string.IsNullOrEmpty(element.VertAlign))
                element.VertAlign = "top";

            var alignment = ParseUtils.ParseAlignment(element.Align, element.VertAlign);
            if (alignment.HasValue)
                gridLayout.childAlignment = alignment.Value;

            if (!string.IsNullOrEmpty(element.Rows) &&
                !string.IsNullOrEmpty(element.Columns))
            {
                Debug.LogError($"You cannot set both ROWS and COLS in GridLayout!");
            }
            else if (!string.IsNullOrEmpty(element.Rows))
            {
                gridLayout.constraint = GridLayoutGroup.Constraint.FixedRowCount;
                gridLayout.constraintCount = context.ParseInt(element.Rows);
            }
            else if (!string.IsNullOrEmpty(element.Columns))
            {
                gridLayout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
                gridLayout.constraintCount = context.ParseInt(element.Columns);
            }
            else
            {
                gridLayout.constraint = GridLayoutGroup.Constraint.Flexible;
            }

            if (!string.IsNullOrEmpty(element.Axis))
            {
                gridLayout.startAxis = (element.Axis == "vertical")
                    ? GridLayoutGroup.Axis.Vertical
                    : GridLayoutGroup.Axis.Horizontal;
            }

            if (!string.IsNullOrEmpty(element.FitSize))
            {
                var fitsize = context.ParseString(element.FitSize);
                var fitter = go.AddComponent<ContentSizeFitter>();
                fitter.verticalFit = fitsize == "both" || fitsize == "vertical" ? ContentSizeFitter.FitMode.PreferredSize : ContentSizeFitter.FitMode.Unconstrained;
                fitter.horizontalFit = fitsize == "both" || fitsize == "horizontal" ? ContentSizeFitter.FitMode.PreferredSize : ContentSizeFitter.FitMode.Unconstrained;
            }
            
            return go;
        }

        protected override void PostInstall(GameObject go, GridLayoutElement element, IReadOnlyLayoutContext context)
        {
            go.GetComponent<GridLayoutGroup>().CalculateLayoutInputVertical();
            go.GetComponent<GridLayoutGroup>().CalculateLayoutInputHorizontal();
        }
    }
}