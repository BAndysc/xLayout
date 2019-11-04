using UnityEngine;
using xLayout.Animations;
using xLayout.Definitions;

namespace xLayout.TypesConstructors
{
    [TriggerConstructor]
    public class OnPointerUpTriggerConstructor : TriggerConstructor<OnPointerUpTriggerElement>
    {
        protected override UITrigger Install(GameObject go, OnPointerUpTriggerElement element, IReadOnlyLayoutContext context, IReadOnlyAnimationContext animationContext)
        {
            return go.AddComponent<OnPointerUpPlayAnimation>();
        }
    }
}