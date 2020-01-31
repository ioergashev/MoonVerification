using DG.Tweening;

namespace Moon.Asyncs
{
    public class TweenAwaiter : AsyncAwaiter
    {
        private Tween _tween;

        public TweenAwaiter(Tween tween)
        {
            _tween = tween;
            _tween.onComplete = Complete;
        }

        protected override void Completed()
        {
            if (_tween == null) return;
            _tween.onComplete = null;
            _tween = null;
        }
    }

    public static class TweenExtensions
    {
        public static AsyncAwaiter GetAwaiter(this Tween tween)
        {
            return new TweenAwaiter(tween);
        }
    }
}
