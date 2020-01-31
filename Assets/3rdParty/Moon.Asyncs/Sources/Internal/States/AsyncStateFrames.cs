namespace Moon.Asyncs.Internal
{
    internal class AsyncStateFrames : AsyncState
    {
        int _frames;
        public AsyncStateFrames(int frames)
        {
            _frames = frames;
        }

        public override void Terminate()
        {
            _frames = 0;
        }

        public override void Update()
        {
            _frames--;
            isComplete = _frames <= 0f;
        }
    }
}