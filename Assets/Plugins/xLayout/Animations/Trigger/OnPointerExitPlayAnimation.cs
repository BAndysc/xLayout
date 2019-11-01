using UnityEngine;
using UnityEngine.EventSystems;

namespace xLayout.Animations
{
    public class OnPointerExitPlayAnimation : UITrigger, IPointerExitHandler
    {        
        public void OnPointerExit(PointerEventData eventData)
        {
            TryPlay();
        }
    }
}