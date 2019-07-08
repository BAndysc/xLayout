using UnityEngine;
using xLayout.Definitions;

namespace xLayout.TypesConstructors
{
    public class EmptyConstructor : TypeConstructor<EmptyElement>
    {
        protected override GameObject Install(GameObject go, EmptyElement element)
        {
            return go;
        }
    }
}