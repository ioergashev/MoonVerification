using System;
using System.Threading.Tasks;

namespace Moon.Asyncs.Internal.Commands
{
    internal class AsyncCommandTaskArgs<TArgs> : AsyncCommand
    {
        private TArgs _args;
        private Func<TArgs, Task> _command;

        public AsyncCommandTaskArgs(Func<TArgs, Task> command, TArgs args)
        {
            _command = command;
            _args = args;
        }

        protected override AsyncState CallStart()
        {
            return new AsyncStateTask(_command(_args));
        }
    }
}
