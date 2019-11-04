using UnityEngine;
using xLayout.Animations;
using xLayout.Definitions;

namespace xLayout.TypesConstructors
{
    [TriggerConstructor]
    public class OnPointerEnterTriggerConstructor : TriggerConstructor<OnPointerEnterTriggerElement>
    {
        protected override UITrigger Install(GameObject go, OnPointerEnterTriggerElement element, IReadOnlyLayoutContext context, IReadOnlyAnimationContext animationContext)
        {
            return go.AddComponent<OnPointerEnterPlayAnimation>();
        }
    }
}