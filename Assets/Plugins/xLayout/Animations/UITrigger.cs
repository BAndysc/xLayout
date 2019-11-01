using UnityEngine;

namespace xLayout.Animations
{
    public abstract class UITrigger : MonoBehaviour
    {
        public UICondition[] conditions;
        
        public UIAnimation animation;

        protected void TryPlay()
        {
            if (ConditionsMet())
            animation.Animate();
        }

        private bool ConditionsMet()
        {
            foreach (var condition in conditions)
            {
                if (!condition.IsMet())
                    return false;
            }

            return true;
        }
    }
}