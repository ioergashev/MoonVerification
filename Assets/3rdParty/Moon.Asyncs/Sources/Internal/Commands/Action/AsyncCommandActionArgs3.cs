using System;

namespace Moon.Asyncs.Internal.Commands
{
    internal class AsyncCommandActionArgs3<TArgs1, TArgs2, TArgs3> : AsyncCommand
    {
        private readonly Action<TArgs1, TArgs2, TArgs3> _command;
        private readonly TArgs1 _args1;
        private readonly TArgs2 _args2;
        private readonly TArgs3 _args3;

        public AsyncCommandActionArgs3(Action<TArgs1, TArgs2, TArgs3> command, TArgs1 args1, TArgs2 args2, TArgs3 args3, CommandAttributes attributes)
        {
            _command = command;
            _args1 = args1;
            _args2 = args2;
            _args3 = args3;
            this.attributes = attributes;
        }

        protected override AsyncState CallStart()
        {
            _command(_args1, _args2, _args3);
            return new AsyncStateEmpty();
        }
    }
}
