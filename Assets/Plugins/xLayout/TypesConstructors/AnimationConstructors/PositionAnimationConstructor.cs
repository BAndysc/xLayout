using UnityEngine;
using xLayout.Animations;
using xLayout.Definitions;

namespace xLayout.TypesConstructors
{
    [AnimationConstructor]
    public class PositionAnimationConstructor : AnimationConstructor<PositionAnimationElement>
    {
        protected override UIAnimation Install(GameObject go, PositionAnimationElement element, IReadOnlyLayoutContext context)
        {
            var positionAnimation = go.AddComponent<UIPositionAnimation>();
            positionAnimation.Setup(go.GetComponent<RectTransform>(),
                context.ParseVector2(element.Offset), context.ParseFloat(element.Speed, 1));
            return positionAnimation;
        }
    }
}