using System;

namespace Moon.Asyncs.Internal.Commands
{
    internal class AsyncCommandActionArgs<TArgs> : AsyncCommand
    {
        private readonly Action<TArgs> _command;
        private readonly TArgs _args;

        public AsyncCommandActionArgs(Action<TArgs> command, TArgs args, CommandAttributes attributes)
        {
            _command = command;
            _args = args;
            this.attributes = attributes;
        }

        protected override AsyncState CallStart()
        {
            _command(_args);
            return new AsyncStateEmpty();
        }
    }
}
