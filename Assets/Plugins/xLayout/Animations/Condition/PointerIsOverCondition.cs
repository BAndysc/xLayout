using UnityEngine.EventSystems;

namespace xLayout.Animations
{
    public class PointerIsOverCondition : UICondition, IPointerEnterHandler, IPointerExitHandler
    {
        private bool isOver;
        
        public override bool IsMet()
        {
            return isOver;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            isOver = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            isOver = false;
        }
    }
}