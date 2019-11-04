using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using xLayout.Definitions;

namespace xLayout.TypesConstructors
{
    public static class Constructors
    {
        private static Dictionary<Type, TypeConstructor> constructors;
        private static Dictionary<Type, AnimationConstructor> animationConstructors;
        private static Dictionary<Type, TriggerConstructor> triggerConstructors;

        private static IEnumerable<Type> GetClassesWithAttribute<T>() where T : Attribute
        {
            return typeof(Constructors).Assembly.GetTypes()
                .Where(t => t.GetCustomAttribute<T>() != null);
        }

        private static void InitDict<T, K>(Dictionary<Type, T> dict) where K : Attribute where T : class
        {
            foreach (var type in GetClassesWithAttribute<K>())
            {
                var instance = Activator.CreateInstance(type) as T;
                var property = instance.GetType().GetProperty("ConstructedType"); // hack
                var constructedType = property.GetValue(instance) as Type;
                dict[constructedType] = instance;
            }            
        }
        
        static Constructors()
        {
            constructors = new Dictionary<Type, TypeConstructor>();
            animationConstructors = new Dictionary<Type, AnimationConstructor>();
            triggerConstructors = new Dictionary<Type, TriggerConstructor>();

            InitDict<TypeConstructor, TypeConstructorAttribute>(constructors);
            InitDict<AnimationConstructor, AnimationConstructorAttribute>(animationConstructors);
            InitDict<TriggerConstructor, TriggerConstructorAttribute>(triggerConstructors);            
        }

        public static TypeConstructor GetConstructor(Type type)
        {
            if (!constructors.TryGetValue(type, out var constructor))
                throw new Exception($"Cannot find constructor for type {type}");
            return constructor;
        }
        
        public static AnimationConstructor GetAnimationConstructor<T>(T t) where T : AnimationElement
        {
            if (!animationConstructors.TryGetValue(t.GetType(), out var constructor))
                throw new Exception($"Cannot find animation constructor for type {t.GetType()}");
            return constructor;
        }
        
        public static TriggerConstructor GetTriggerConstructor<T>(T t) where T : TriggerElement
        {
            if (!triggerConstructors.TryGetValue(t.GetType(), out var constructor))
                throw new Exception($"Cannot find trigger constructor for type {t.GetType()}");
            return constructor;
        }
    }
}