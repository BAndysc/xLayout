using System;
using UnityEngine;
using xLayout.Animations;
using xLayout.Definitions;

namespace xLayout.TypesConstructors
{
    public abstract class TriggerConstructor
    {
        public abstract Type ConstructedType { get; }

        public abstract UITrigger Install(GameObject gameObject, TriggerElement element, IReadOnlyLayoutContext context, IReadOnlyAnimationContext animationContext);
    }
    
    public abstract class TriggerConstructor<T> : TriggerConstructor where T : TriggerElement
    {
        public override Type ConstructedType => typeof(T);

        public override UITrigger Install(GameObject gameObject, TriggerElement element, IReadOnlyLayoutContext context, IReadOnlyAnimationContext animationContext)
        {
            return Install(gameObject, element as T, context, animationContext);
        }
        
        protected abstract UITrigger Install(GameObject go, T element, IReadOnlyLayoutContext context, IReadOnlyAnimationContext animationContext);        
    }
}