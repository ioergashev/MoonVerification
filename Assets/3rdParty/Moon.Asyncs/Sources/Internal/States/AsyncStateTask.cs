using System.Threading.Tasks;

namespace Moon.Asyncs.Internal
{
    internal class AsyncStateTask : AsyncState
    {
        private Task _task;

        public AsyncStateTask(Task task)
        {
            _task = task;
        }

        public override void Terminate()
        {
            // how terminate task?
        }

        public override void Update()
        {
            isComplete = _task.IsCompleted;
        }
    }
}
