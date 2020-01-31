using System.Collections.Generic;
using System.Threading;
using Moon.Asyncs.Internal;

namespace Moon.Asyncs
{
    public sealed class MangoSynchronizationContext: SynchronizationContext
    {
        private const int КAwqInitialCapacity = 20;
        private readonly Queue<WorkRequest> _asyncWorkQueue;
        private readonly int _mainThreadId;
        private bool _paused;

        public MangoSynchronizationContext(int mainThreadId)
        {
            _asyncWorkQueue = new Queue<WorkRequest>(КAwqInitialCapacity);
            _mainThreadId = mainThreadId;
            SynchronizationContextComponent.Initialize(this);
        }

        private MangoSynchronizationContext(Queue<WorkRequest> queue, int mainThreadId)
        {
            _asyncWorkQueue = queue;
            _mainThreadId = mainThreadId;
        }

        // Send will process the call synchronously. If the call is processed on the main thread, we'll invoke it
        // directly here. If the call is processed on another thread it will be queued up like POST to be executed
        // on the main thread and it will wait. Once the main thread processes the work we can continue
        public override void Send(SendOrPostCallback callback, object state)
        {
            if (_paused) return;
            if (_mainThreadId == Thread.CurrentThread.ManagedThreadId)
            {
                callback(state);
            }
            else
            {
                using (var waitHandle = new ManualResetEvent(false))
                {
                    lock (_asyncWorkQueue)
                    {
                        _asyncWorkQueue.Enqueue(new WorkRequest(callback, state, waitHandle));
                    }
                    waitHandle.WaitOne();
                }
            }
        }

        // Post will add the call to a task list to be executed later on the main thread then work will continue asynchronously
        public override void Post(SendOrPostCallback callback, object state)
        {
            if (_paused) return;
            lock (_asyncWorkQueue)
            {
                _asyncWorkQueue.Enqueue(new WorkRequest(callback, state));
            }
        }

        public void Pause()
        {
            _paused = true;
        }

        public void Resume()
        {
            _paused = false;
        }

        public void Clear()
        {
            lock (_asyncWorkQueue)
            {
                _asyncWorkQueue.Clear();
            }
        }

        // CreateCopy returns a new UnitySynchronizationContext object, but the queue is still shared with the original
        public override SynchronizationContext CreateCopy()
        {
            lock (_asyncWorkQueue)
            {
                return new MangoSynchronizationContext(_asyncWorkQueue, _mainThreadId);
            }
        }

        // Update will execute tasks off the task list
        public void Update()
        {
            if (_paused) return;
            lock (_asyncWorkQueue)
            {
                int workCount = _asyncWorkQueue.Count;
                for (var i = 0; i < workCount; i++)
                {
                    WorkRequest work = _asyncWorkQueue.Dequeue();
                    work.Invoke();
                }
            }
        }
        
        private struct WorkRequest
        {
            private readonly SendOrPostCallback _delegateCallback;
            private readonly object _delegateState;
            private readonly ManualResetEvent _waitHandle;

            public WorkRequest(SendOrPostCallback callback, object state, ManualResetEvent waitHandle = null)
            {
                _delegateCallback = callback;
                _delegateState = state;
                _waitHandle = waitHandle;
            }

            public void Invoke()
            {
                _delegateCallback(_delegateState);
                _waitHandle?.Set();
            }
        }
    }
}
