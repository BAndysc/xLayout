using UnityEngine;
using xLayout.Animations;
using xLayout.Definitions;

namespace xLayout.TypesConstructors
{
    [TriggerConstructor]
    public class OnPointerExitTriggerConstructor : TriggerConstructor<OnPointerExitTriggerElement>
    {
        protected override UITrigger Install(GameObject go, OnPointerExitTriggerElement element, IReadOnlyLayoutContext context, IReadOnlyAnimationContext animationContext)
        {
            return go.AddComponent<OnPointerExitPlayAnimation>();
        }
    }
}