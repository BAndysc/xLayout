using UnityEngine;
using xLayout.Definitions;

namespace xLayout.TypesConstructors
{
    public class ListViewConstructor : TypeConstructor<ListViewElement>
    {
        protected override GameObject Install(GameObject go, ListViewElement element, IReadOnlyLayoutContext context)
        {
            xLayouter.BuildLayout(go, element.ChildItem.Elements, context);
            return go;
        }
    }
}