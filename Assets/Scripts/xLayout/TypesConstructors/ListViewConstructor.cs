using UnityEngine;
using xLayout.Definitions;

namespace xLayout.TypesConstructors
{
    public class ListViewConstructor : TypeConstructor<ListViewElement>
    {
        protected override GameObject Install(GameObject go, ListViewElement element)
        {
            xLayouter.BuildLayout(go, element.ChildItem.Elements);
            return go;
        }
    }
}