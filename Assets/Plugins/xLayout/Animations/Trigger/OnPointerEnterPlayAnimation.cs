using UnityEngine;
using UnityEngine.EventSystems;

namespace xLayout.Animations
{
    public class OnPointerEnterPlayAnimation : UITrigger, IPointerEnterHandler
    {
        public void OnPointerEnter(PointerEventData eventData)
        {
            TryPlay();
        }
    }
}