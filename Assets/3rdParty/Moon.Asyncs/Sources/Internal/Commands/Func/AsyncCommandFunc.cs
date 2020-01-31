using System;

namespace Moon.Asyncs.Internal.Commands
{
    internal class AsyncCommandFunc: AsyncCommand
    {
        private Func<AsyncState> _command;

        public AsyncCommandFunc(Func<AsyncState> command)
        {
            _command = command;
        }

        protected override AsyncState CallStart()
        {
            return _command();
        }
    }
}
