using System;

namespace Moon.Asyncs.Internal.Commands
{
    internal class AsyncCommandAction : AsyncCommand
    {
        private readonly Action _command;

        public AsyncCommandAction(Action command, CommandAttributes attributes)
        {
            _command = command;
            this.attributes = attributes;
        }

        protected override AsyncState CallStart()
        {
            _command();
            return new AsyncStateEmpty();
        }
    }
}
