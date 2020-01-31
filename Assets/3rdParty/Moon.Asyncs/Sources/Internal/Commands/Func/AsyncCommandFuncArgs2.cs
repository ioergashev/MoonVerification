using System;

namespace Moon.Asyncs.Internal.Commands
{
    internal class AsyncCommandFuncArgs2<TArgs1, TArgs2> : AsyncCommand
    {
        private Func<TArgs1, TArgs2, AsyncState> _command;
        private TArgs1 _args1;
        private TArgs2 _args2;

        public AsyncCommandFuncArgs2(Func<TArgs1, TArgs2, AsyncState> command, TArgs1 args1, TArgs2 args2)
        {
            _command = command;
            _args1 = args1;
            _args2 = args2;
        }

        protected override AsyncState CallStart()
        {
            return _command(_args1, _args2);
        }
    }
}
