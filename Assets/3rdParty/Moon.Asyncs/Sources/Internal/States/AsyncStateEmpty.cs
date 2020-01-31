namespace Moon.Asyncs.Internal
{
    internal class AsyncStateEmpty : AsyncState
    {
        public override void Terminate()
        {
        }

        public override void Update()
        {
            isComplete = true;
        }
    }
}
