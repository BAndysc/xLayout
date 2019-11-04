using UnityEngine;
using xLayout.Animations;
using xLayout.Definitions;

namespace xLayout.TypesConstructors
{
    [TriggerConstructor]
    public class OnPointerDownTriggerConstructor : TriggerConstructor<OnPointerDownTriggerElement>
    {
        protected override UITrigger Install(GameObject go, OnPointerDownTriggerElement element, IReadOnlyLayoutContext context, IReadOnlyAnimationContext animationContext)
        {
            return go.AddComponent<OnPointerDownPlayAnimation>();
        }
    }
}