using System;
using UniRx;
using UnityEngine;
using xLayout.Example;

namespace xLayout.Animations
{
    public class UIPositionAnimation : UIAnimation
    {
        [SerializeField] private RectTransform rt;

        [SerializeField] private Vector2 offset;
        
        [SerializeField] private float speed;

        private Vector2 baseAnchoredPosition;
        private Vector2 destAnchoredPosition;
        
        protected override void OnAwake()
        {
            baseAnchoredPosition = rt.anchoredPosition;
            destAnchoredPosition = baseAnchoredPosition + offset;
        }

        protected override IDisposable PlayAnimation()
        {
            return new FollowValue<Vector2>(rt.anchoredPosition, destAnchoredPosition,
                (current, target, deltaTime) => Vector2.MoveTowards(current, target, deltaTime * speed)).Subscribe(t => rt.anchoredPosition = t, AnimationCompleted);
        }

        protected override void PlayInstantAnimation()
        {
            rt.anchoredPosition = destAnchoredPosition;
        }

        internal void Setup(RectTransform rt, Vector2 offset, float speed)
        {
            Debug.Assert(!Application.isPlaying);
            this.rt = rt;
            this.offset = offset;
            this.speed = speed;
        }
    }
}