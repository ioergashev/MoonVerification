using System;

namespace Moon.Asyncs.Internal.Commands
{
    internal class AsyncCommandActionArgs2<TArgs1, TArgs2> : AsyncCommand
    {
        private readonly Action<TArgs1, TArgs2> _command;
        private readonly TArgs1 _args1;
        private readonly TArgs2 _args2;

        public AsyncCommandActionArgs2(Action<TArgs1, TArgs2> command, TArgs1 args1, TArgs2 args2, CommandAttributes attributes)
        {
            _command = command;
            _args1 = args1;
            _args2 = args2;
            this.attributes = attributes;
        }

        protected override AsyncState CallStart()
        {
            _command(_args1, _args2);
            return new AsyncStateEmpty();
        }
    }
}
