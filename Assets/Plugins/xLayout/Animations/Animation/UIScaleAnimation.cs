using System;
using UniRx;
using UnityEngine;
using xLayout.Example;

namespace xLayout.Animations
{
    public class UIScaleAnimation : UIAnimation
    {
        [SerializeField] private RectTransform rt;

        [SerializeField] private Vector3 destScale;
        
        [SerializeField] private float speed;

        protected override IDisposable PlayAnimation()
        {
            return new FollowValue<Vector3>(rt.localScale, destScale,
                (current, target, deltaTime) => Vector3.MoveTowards(current, target, deltaTime * speed)).Subscribe(t => rt.localScale = t);
        }

        internal void Setup(RectTransform rt, Vector3 destScale, float speed)
        {
            Debug.Assert(!Application.isPlaying);
            this.rt = rt;
            this.destScale = destScale;
            this.speed = speed;
        }
    }
}