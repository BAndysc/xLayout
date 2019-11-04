using UnityEngine;
using xLayout.Animations;
using xLayout.Definitions;

namespace xLayout.TypesConstructors
{
    [TriggerConstructor]
    public class OnEnableTriggerConstructor : TriggerConstructor<OnEnableTriggerElement>
    {
        protected override UITrigger Install(GameObject go, OnEnableTriggerElement element, IReadOnlyLayoutContext context, IReadOnlyAnimationContext animationContext)
        {
            return go.AddComponent<OnEnablePlayAnimation>();
        }
    }
}