using System;

namespace Moon.Asyncs.Internal.Commands
{
    internal class AsyncCommandFuncArgs3<TArgs1, TArgs2, TArgs3> : AsyncCommand
    {
        private readonly Func<TArgs1, TArgs2, TArgs3, AsyncState> _command;
        private readonly TArgs1 _args1;
        private readonly TArgs2 _args2;
        private readonly TArgs3 _args3;

        public AsyncCommandFuncArgs3(Func<TArgs1, TArgs2, TArgs3, AsyncState> command, TArgs1 args1, TArgs2 args2, TArgs3 args3)
        {
            _command = command;
            _args1 = args1;
            _args2 = args2;
            _args3 = args3;
        }

        protected override AsyncState CallStart()
        {
            return _command(_args1, _args2, _args3);
        }
    }
}