using System;
using UnityEngine;

namespace Moon.Asyncs.Internal
{
    internal class AsyncStateOperation : AsyncState
    {
        private Func<AsyncOperation> _awaitFunc;
        private AsyncStateInfo _info;
        private AsyncOperation _asyncOperation;

        public AsyncStateOperation(Func<AsyncOperation> awaitFunc)
        {
            _awaitFunc = awaitFunc;
            _info = new AsyncStateInfo();
        }

        public override void Start()
        {
            _asyncOperation = _awaitFunc();
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
