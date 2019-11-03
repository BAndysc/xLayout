using System;
using UniRx;
using UnityEditor;
using UnityEngine;
using xLayout.Example;

namespace xLayout.Animations
{
    public class CanvasAlphaAnimation : UIAnimation
    {
        [SerializeField] private CanvasGroup canvas;

        [SerializeField] private float destValue;

        protected override IDisposable PlayAnimation()
        {
            return new FollowValue<float>(canvas.alpha, destValue, Mathf.MoveTowards).Subscribe(t => canvas.alpha = t, AnimationCompleted);
        }

        protected override void PlayInstantAnimation()
        {
            canvas.alpha = destValue;
        }

        internal void Setup(CanvasGroup canvas, float value)
        {
            Debug.Assert(!Application.isPlaying);
            this.canvas = canvas;
            destValue = value;
        }
    }
}