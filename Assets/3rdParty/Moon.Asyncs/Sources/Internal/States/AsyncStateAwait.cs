using System;

namespace Moon.Asyncs.Internal
{
    internal class AsyncStateAwait : AsyncState
    {
        private Action<AsyncStateInfo> _awaitFunc;
        private AsyncStateInfo _info;

        public AsyncStateAwait(Action<AsyncStateInfo> awaitFunc)
        {
            _awaitFunc = awaitFunc;
            _info = new AsyncStateInfo();
        }

        public override void Terminate()
        {
            _info.IsTerminated = true;
        }
        
        public override void Update()
        {
            _info.IsComplete = isComplete;
            _awaitFunc(_info);
            isComplete = _info.IsComplete;
        }
    }

    internal class AsyncStateAwait<T> : AsyncState
    {
        private Action<T, AsyncStateInfo> _awaitFunc;
        private T _args;
        private readonly AsyncStateInfo _info;

        public AsyncStateAwait(Action<T, AsyncStateInfo> awaitFunc, T args)
        {
            _awaitFunc = awaitFunc;
            _args = args;
            _info = new AsyncStateInfo();
        }

        public override void Terminate()
        {
            _info.IsTerminated = true;
        }

        public override void Update()
        {
            _info.IsComplete = isComplete;
            _awaitFunc(_args, _info);
            isComplete = _info.IsComplete;
        }
    }
}
