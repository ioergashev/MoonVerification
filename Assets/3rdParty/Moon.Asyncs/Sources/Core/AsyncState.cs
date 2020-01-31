using System;
using System.Collections;
using System.Threading.Tasks;
using Moon.Asyncs.Internal;
using UnityEngine;

namespace Moon.Asyncs
{
    public abstract class AsyncState
    {
        public bool isComplete { get; protected set; }
        public virtual void Start() { }
        public abstract void Update();
        public abstract void Terminate();
        public Action onComplete;
        public Action onTerminate;

        /// <summary>
        /// Creates awaiter for async/await pattern
        /// </summary>
        /// <returns>awaiter for async/await pattern</returns>
        public AsyncAwaiter GetAwaiter()
        {
            return new AsyncStateAwaiter(this);
        }

        /// <summary>
        /// Create coroutine for this state
        /// </summary>
        /// <returns></returns>
        public IEnumerator ToCoroutine()
        {
            while (!isComplete)
            {
                yield return 0;
            }
        }

        public void ReportComplete()
        {
            onComplete?.Invoke();
        }

        public void ReportTerminate()
        {
            onTerminate?.Invoke();
        }


        protected static AsyncState Empty()
        {
            return new AsyncStateEmpty();
        }

        protected static AsyncState Never()
        {
            return new AsyncStateNever();
        }
        
        protected static AsyncState FromTimer(float time, bool independent)
        {
            return new AsyncStateTimer(time, independent);
        }

        protected static AsyncState FromFrames(int frames)
        {
            return new AsyncStateFrames(frames);
        }

        protected static AsyncState FromAwait(Action<AsyncStateInfo> awaitFunc)
        {
            return new AsyncStateAwait(awaitFunc);
        }

        protected static AsyncState FromAwait<TArgs>(Action<TArgs, AsyncStateInfo> awaitFunc, TArgs args)
        {
            return new AsyncStateAwait<TArgs>(awaitFunc, args);
        }

        protected static AsyncState FromAsyncOperation(Func<AsyncOperation> asyncFunc)
        {
            return new AsyncStateOperation(asyncFunc);
        }

        protected static AsyncState FromAsyncOperation<TArgs>(Func<TArgs, AsyncOperation> asyncFunc, TArgs args)
        {
            return new AsyncStateOperationArgs<TArgs>(asyncFunc, args);
        }

        protected static AsyncState FromTask(Task task)
        {
            return new AsyncStateTask(task);
        }
    }
}
