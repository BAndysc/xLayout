using UnityEngine;
using xLayout.Animations;
using xLayout.Definitions;

namespace xLayout.TypesConstructors
{
    [TriggerConstructor]
    public class OnAnimationFinishedTriggerConstructor : TriggerConstructor<OnAnimationFinishedTriggerElement>
    {
        protected override UITrigger Install(GameObject go, OnAnimationFinishedTriggerElement element, IReadOnlyLayoutContext context, IReadOnlyAnimationContext animationContext)
        {
            var finishTriggerComponent = go.AddComponent<OnAnimationFinishedPlayAnimation>();

            var otherAnimation = animationContext.FindAnimation(element.Animation);
            
            if (otherAnimation == null)
                Debug.LogError("Cannot find animation with key " + element.Animation);

            finishTriggerComponent.otherAnimation = otherAnimation;

            return finishTriggerComponent;
        }
    }
}