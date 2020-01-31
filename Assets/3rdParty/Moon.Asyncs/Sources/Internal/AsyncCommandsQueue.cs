using System.Collections.Generic;
using Moon.Asyncs.Internal.Commands;

namespace Moon.Asyncs.Internal
{
    internal class AsyncCommandsQueue
    {
        private Queue<AsyncCommandCollection> _commands = new Queue<AsyncCommandCollection>();
        private AsyncCommandCollection _last;

        public AsyncCommandCollection Last => _last;
        public int Count => _commands.Count;

        public void Enqueue(AsyncCommandCollection command)
        {
            _last = command;
            _commands.Enqueue(command);
        }

        public AsyncCommandCollection Dequeue()
        {
            var result = _commands.Dequeue();
            if (_commands.Count == 0)
            {
                _last = null;
            }
            return result;
        }

        public void Clear()
        {
            _commands.Clear();
            _last = null;
        }
    }

}
