using xLayout.Animations;

namespace xLayout.TypesConstructors
{
    public interface IReadOnlyAnimationContext
    {
        UIAnimation FindAnimation(string key);
    }
}