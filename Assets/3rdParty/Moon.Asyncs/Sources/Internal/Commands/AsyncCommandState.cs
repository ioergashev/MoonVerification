namespace Moon.Asyncs.Internal.Commands
{
    internal class AsyncCommandState : AsyncCommand
    {
        private AsyncState _state;
        
        public AsyncCommandState(AsyncState state)
        {
            _state = state;
        }

        protected override AsyncState CallStart()
        {
            return _state;
        }
    }
}
