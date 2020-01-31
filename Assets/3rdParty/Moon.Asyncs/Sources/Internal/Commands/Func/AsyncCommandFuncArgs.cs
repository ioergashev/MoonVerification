using System;

namespace Moon.Asyncs.Internal.Commands
{
    internal class AsyncCommandFuncArgs<TArgs> : AsyncCommand
    {
        private Func<TArgs, AsyncState> _command;
        private TArgs _args;

        public AsyncCommandFuncArgs(Func<TArgs, AsyncState> command, TArgs args)
        {
            _command = command;
            _args = args;
        }

        protected override AsyncState CallStart()
        {
            return _command(_args);
        }
    }
}
