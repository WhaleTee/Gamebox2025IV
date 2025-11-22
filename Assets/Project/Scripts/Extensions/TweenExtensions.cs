using DG.Tweening;

namespace Extensions
{
    public static class TweenExtensions
    {
        public static Tween Preserve(this Tween tween, string tweenID) => tween
                .SetAutoKill(false)
                .Pause()
                .SetRecyclable(false)
                .SetId(tweenID);
    }
}