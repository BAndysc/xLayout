using System;
using System.Collections.Generic;
using UnityEngine;

namespace xLayout.Animations
{
    public class UIAnimator : MonoBehaviour
    {
        private Dictionary<Type, System.IDisposable> animations = new Dictionary<Type, IDisposable>();

        public void StartAnimation(Type type, System.IDisposable animation)
        {
            if (animations.TryGetValue(type, out var anim))
                anim.Dispose();

            animations[type] = animation;
        }
    }
}