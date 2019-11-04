using UnityEngine;
using xLayout.Animations;
using xLayout.Definitions;

namespace xLayout.TypesConstructors
{
    [AnimationConstructor]
    public class ScaleAnimationConstructor : AnimationConstructor<ScaleAnimationElement>
    {
        protected override UIAnimation Install(GameObject go, ScaleAnimationElement element, IReadOnlyLayoutContext context)
        {
            var scaleAnimation = go.AddComponent<UIScaleAnimation>();
            scaleAnimation.Setup(go.GetComponent<RectTransform>(),
                context.ParseVector3(element.DestValue), context.ParseFloat(element.Speed, 1));

            return scaleAnimation;
        }
    }
}