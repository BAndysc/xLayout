using System.Collections.Generic;
using xLayout.Animations;

namespace xLayout.TypesConstructors
{
    public class AnimationContext : IReadOnlyAnimationContext
    {
        private Dictionary<string, UIAnimation> animations = new Dictionary<string, UIAnimation>();
        
        public UIAnimation FindAnimation(string key)
        {
            if (animations.TryGetValue(key, out var anim))
                return anim;

            return null;
        }

        public void AddAnimation(string key, UIAnimation animation)
        {
            animations[key] = animation;
        }
    }
}