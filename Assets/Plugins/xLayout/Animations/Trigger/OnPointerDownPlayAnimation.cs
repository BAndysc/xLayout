using UnityEngine.EventSystems;

namespace xLayout.Animations
{
    public class OnPointerDownPlayAnimation : UITrigger, IPointerDownHandler
    {
        public void OnPointerDown(PointerEventData eventData)
        {
            TryPlay();
        }
    }
}