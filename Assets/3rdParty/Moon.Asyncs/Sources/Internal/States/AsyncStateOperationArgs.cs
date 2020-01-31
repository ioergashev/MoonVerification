using System;
using UnityEngine;

namespace Moon.Asyncs.Internal
{
    internal class AsyncStateOperationArgs<TArgs> : AsyncState
    {
        private Func<TArgs, AsyncOperation> _awaitFunc;
        private TArgs _args;

        private AsyncStateInfo _info;
        private AsyncOperation _asyncOperation;

        public AsyncStateOperationArgs(Func<TArgs, AsyncOperation> awaitFunc, TArgs args)
        {
            _awaitFunc = awaitFunc;
            _args = args;
            _info = new AsyncStateInfo();
        }

        public override void Start()
        {
            _asyncOperation = _awaitFunc(_args);
        }

        public override void Terminate()
        {
            _info.IsTerminated = true;
        }

        public override void Update()
        {
            _info.IsComplete = isComplete;
            _info.IsComplete = _asyncOperation.isDone;
            isComplete = _info.IsComplete;
        }
    }
}
