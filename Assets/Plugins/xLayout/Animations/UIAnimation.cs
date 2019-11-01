using UniRx;
using UniRx.InternalUtil;
using UnityEngine;
using UnityEngine.EventSystems;

namespace xLayout.Animations
{
    [RequireComponent(typeof(UIAnimator))]
    public abstract class UIAnimation : MonoBehaviour
    {
        private UIAnimator animator;
        
        private void Awake()
        {
            animator = GetComponent<UIAnimator>();
        }

        protected abstract System.IDisposable PlayAnimation();
        
        public void Animate()
        {
            var anim = PlayAnimation();
            animator.StartAnimation(GetType(), anim);
        }
    }
}