using System;
using UnityEngine;
using xLayout.Definitions;

namespace xLayout.TypesConstructors
{
    public abstract class TypeConstructor
    {
        public abstract Type ConstructedType { get; }

        public abstract GameObject Install(GameObject gameObject, BaseElement element, IReadOnlyLayoutContext context);
        
        public abstract void PostInstall(GameObject gameObject, BaseElement element, IReadOnlyLayoutContext context);
    }
    
    public abstract class TypeConstructor<T> : TypeConstructor where T : BaseElement
    {
        public override Type ConstructedType => typeof(T);

        public override GameObject Install(GameObject gameObject, BaseElement element, IReadOnlyLayoutContext context)
        {
            return Install(gameObject, element as T, context);
        }
        
        public override void PostInstall(GameObject gameObject, BaseElement element, IReadOnlyLayoutContext context)
        {
            PostInstall(gameObject, element as T, context);
        }

        protected abstract GameObject Install(GameObject go, T element, IReadOnlyLayoutContext context);
        
        protected virtual void PostInstall(GameObject go, T element, IReadOnlyLayoutContext context) { }
    }
}