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

        private bool awoken;
        
        public ReactiveCommand<Unit> AnimationFinished { get; } = new ReactiveCommand<Unit>();
        
        private void Awake()
        {
            if (awoken)
                return;

            awoken = true;
            
            animator = GetComponent<UIAnimator>();
            
            OnAwake();
        }

        private void OnDestroy()
        {
            AnimationFinished.Dispose();
        }

        protected abstract System.IDisposable PlayAnimation();
        protected abstract void PlayInstantAnimation();
        
        public void Animate()
        {
            Awake();
            var anim = PlayAnimation();
            animator.StartAnimation(GetType(), anim);
        }

        public void FastForward()
        {
            Awake();
            PlayInstantAnimation();
            animator.StartAnimation(GetType(), Disposable.Empty);
            AnimationCompleted();
        }

        protected void AnimationCompleted()
        {
            AnimationFinished.Execute(Unit.Default);
        }
        
        protected virtual void OnAwake()
        {
            
        }
    }
}