using UnityEngine;
using UnityEngine.UI;
using xLayout.Definitions;

namespace xLayout.TypesConstructors
{
    public class ScrollConstructor : TypeConstructor<ScrollElement>
    {
        protected override GameObject Install(GameObject go, ScrollElement element)
        {
            var scroll = go.AddComponent<ScrollRect>();
            var viewPort = new GameObject($"Viewport ({go.name})");

            viewPort.transform.parent = scroll.transform;

            var viewPortRect = viewPort.AddComponent<RectTransform>();

            viewPortRect.localScale = Vector3.one;

            viewPortRect.anchorMin = Vector2.zero;
            viewPortRect.anchorMax = Vector2.one;

            viewPortRect.offsetMax =
                viewPortRect.offsetMin = Vector2.zero;
            
            //trans = viewPortRect;

            scroll.viewport = viewPortRect;

            var mask = viewPort.gameObject.AddComponent<RectMask2D>();

            return viewPort;
        }

        protected override void PostInstall(GameObject go, ScrollElement element)
        {
            if (go.transform.childCount != 1 || go.transform.GetChild(0).childCount != 1)
                Debug.LogError("Scroll element should have exactly one child");
            else
                go.GetComponent<ScrollRect>().content = go.transform.GetChild(0).GetChild(0).GetComponent<RectTransform>();
        }
    }
}