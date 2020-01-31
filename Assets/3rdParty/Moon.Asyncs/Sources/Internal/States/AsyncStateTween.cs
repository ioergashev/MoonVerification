using DG.Tweening;

namespace Moon.Asyncs.Internal
{
    internal class AsyncStateTween : AsyncState
    {
        private Tween _tween;

        public AsyncStateTween(Tween tween)
        {
            _tween = tween;
            tween.Pause();
        }

        public override void Terminate()
        {
            DOTween.Kill(_tween.id);
        }

        public override void Start()
        {
            base.Start();
            _tween.Play();
        }

        public override void Update()
        {
            isComplete = !_tween.IsPlaying();
        }
    }
}
