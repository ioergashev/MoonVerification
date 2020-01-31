using System;
using System.Threading;

namespace Moon.Asyncs
{
    /// <summary>
    /// Awaiter implementation
    /// </summary>
    /// <typeparam name="T">Type of async command result</typeparam>
    public class AsyncAwaiter<T> : IAsyncAwaiter<T>
    {
        private bool _completed;
        public bool IsCompleted => _completed;

        private T _result;

        private Action _continuation;
             
        public void Complete(T result)
        {
            if (_completed)
            {
                throw new Exception("AsyncAwaiter already completed.");
            }

            _result = result;
            _completed = true;

            if (_continuation != null)
            {
                SynchronizationContext.Current.Post(Continuation, null);
            }
        }

        private void Continuation(object state)
        {
            _continuation();
        }

        public IAsyncAwaiter<T> GetAwaiter()
        {
            return this;
        }

        public T GetResult()
        {
            return _result;
        }

        public void OnCompleted(Action continuation)
        {
            _continuation = continuation;
        }
    }

    /// <summary>
    /// Awaiter implementation for async functions with void result
    /// </summary>
    public class AsyncAwaiter : AsyncAwaiter<VoidTaskResult>
    {
        public void Complete()
        {
            Complete(VoidTaskResult.Empty);
            Completed();
        }

        protected virtual void Completed()
        {
        }
    }
}