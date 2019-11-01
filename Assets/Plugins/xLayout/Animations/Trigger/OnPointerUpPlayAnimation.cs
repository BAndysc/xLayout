using UnityEngine.EventSystems;

namespace xLayout.Animations
{
    public class OnPointerUpPlayAnimation : UITrigger, IPointerUpHandler
    {
        public void OnPointerUp(PointerEventData eventData)
        {
            TryPlay();
        }
    }
}