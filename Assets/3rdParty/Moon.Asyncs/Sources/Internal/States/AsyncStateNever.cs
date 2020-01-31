namespace Moon.Asyncs.Internal
{
    internal class AsyncStateNever : AsyncState
    {
        public override void Terminate()
        {
        }

        public override void Update()
        {
            isComplete = false;
        }
    }
}
