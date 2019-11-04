using UnityEngine;
using xLayout.Animations;
using xLayout.Definitions;

namespace xLayout.TypesConstructors
{
    [AnimationConstructor]
    public class CanvasAlphaAnimationConstructor : AnimationConstructor<CanvasAlphaAnimationElement>
    {
        protected override UIAnimation Install(GameObject go, CanvasAlphaAnimationElement element, IReadOnlyLayoutContext context)
        {
            CanvasGroup cg = go.EnsureComponent<CanvasGroup>();
            var alphaAnimation = go.AddComponent<CanvasAlphaAnimation>();
            alphaAnimation.Setup(cg, context.ParseFloat(element.DestValue));

            return alphaAnimation;
        }
    }
}