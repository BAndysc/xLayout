using System;
using UnityEngine;
using xLayout.Animations;
using xLayout.Definitions;

namespace xLayout.TypesConstructors
{
    public abstract class AnimationConstructor
    {
        public abstract Type ConstructedType { get; }

        public abstract UIAnimation Install(GameObject gameObject, AnimationElement element, IReadOnlyLayoutContext context);
    }
    
    public abstract class AnimationConstructor<T> : AnimationConstructor where T : AnimationElement
    {
        public override Type ConstructedType => typeof(T);

        public override UIAnimation Install(GameObject gameObject, AnimationElement element, IReadOnlyLayoutContext context)
        {
            return Install(gameObject, element as T, context);
        }
        
        protected abstract UIAnimation Install(GameObject go, T element, IReadOnlyLayoutContext context);        
    }
}