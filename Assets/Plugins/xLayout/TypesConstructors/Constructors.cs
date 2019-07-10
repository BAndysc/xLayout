using System;
using System.Collections.Generic;

namespace xLayout.TypesConstructors
{
    public static class Constructors
    {
        private static Dictionary<Type, TypeConstructor> constructors;
        
        static Constructors()
        {
            constructors = new Dictionary<Type, TypeConstructor>();

            var types = new TypeConstructor[]
            {
                new ImageConstructor(),
                new RawImageConstructor(),
                new LabelConstructor(),
                new ListViewConstructor(),
                new ScrollConstructor(),
                new VerticalLayoutConstructor(),
                new HorizontalLayoutConstructor(),
                new EmptyConstructor(),
                new PrefabConstructor(), 
                new GameObjectConstructor(), 
                new GridLayoutConstructor(), 
            };

            foreach (var type in types)
                constructors[type.ConstructedType] = type;
        }

        public static TypeConstructor GetConstructor(Type type)
        {
            if (!constructors.TryGetValue(type, out var constructor))
                throw new Exception($"Cannot find constructor for type {type}");
            return constructor;
        }
    }
}