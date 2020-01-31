using System;
using System.Threading.Tasks;

namespace Moon.Asyncs.Internal.Commands
{
    internal class AsyncCommandTask : AsyncCommand
    {
        private Func<Task> _command;

        public AsyncCommandTask(Func<Task> command)
        {
            _command = command;
        }

        protected override AsyncState CallStart()
        {
            return new AsyncStateTask(_command());
        }
    }
}
