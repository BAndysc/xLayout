using UnityEngine;
using xLayout.Definitions;

namespace xLayout.TypesConstructors
{
    [TypeConstructor]
    public class EmptyConstructor : TypeConstructor<EmptyElement>
    {
        protected override GameObject Install(GameObject go, EmptyElement element, IReadOnlyLayoutContext context)
        {
            return go;
        }
    }
}