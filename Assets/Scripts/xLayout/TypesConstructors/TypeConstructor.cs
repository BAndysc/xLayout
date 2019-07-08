using System;
using UnityEngine;
using xLayout.Definitions;

namespace xLayout.TypesConstructors
{
    public abstract class TypeConstructor
    {
        public abstract Type ConstructedType { get; }

        public abstract GameObject Install(GameObject gameObject, BaseElement element);
        
        public abstract void PostInstall(GameObject gameObject, BaseElement element);
    }
    
    public abstract class TypeConstructor<T> : TypeConstructor where T : BaseElement
    {
        public override Type ConstructedType => typeof(T);

        public override GameObject Install(GameObject gameObject, BaseElement element)
        {
            return Install(gameObject, element as T);
        }
        
        public override void PostInstall(GameObject gameObject, BaseElement element)
        {
            PostInstall(gameObject, element as T);
        }

        protected abstract GameObject Install(GameObject go, T element);
        
        protected virtual void PostInstall(GameObject go, T element) { }
    }
}