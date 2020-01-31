namespace Moon.Asyncs
{
    /// <summary>
    /// Awaiter for async/await pattern.
    /// Use for await any AsyncState
    /// </summary>
    public class AsyncStateAwaiter : AsyncAwaiter
    {
        private AsyncState _state;

        public AsyncStateAwaiter(AsyncState state)
        {
            _state = state;
            _state.onComplete += Complete;
            _state.onTerminate += Complete;
        }

        protected override void Completed()
        {
            if (_state == null) return;
            _state.onComplete -= Complete;
            _state.onTerminate -= Complete;
            _state = null;
        }
    }
}
