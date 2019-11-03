using UniRx;

namespace xLayout.Animations
{
    public class OnAnimationFinishedPlayAnimation : UITrigger
    {
        public UIAnimation otherAnimation;

        private System.IDisposable subscribtion;
        
        private void Awake()
        {
            subscribtion = otherAnimation.AnimationFinished.Subscribe(OnFinished);
        }

        private void OnFinished(Unit unit)
        {
            TryPlay();
        }

        private void OnDestroy()
        {
            subscribtion.Dispose();
        }
    }
}