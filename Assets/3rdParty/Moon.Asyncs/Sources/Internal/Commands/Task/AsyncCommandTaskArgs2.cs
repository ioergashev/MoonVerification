using System;
using System.Threading.Tasks;

namespace Moon.Asyncs.Internal.Commands
{
    internal class AsyncCommandTaskArgs2<TArgs1, TArgs2> : AsyncCommand
    {
        private TArgs1 _args1;
        private TArgs2 _args2;
        private Func<TArgs1, TArgs2, Task> _command;

        public AsyncCommandTaskArgs2(Func<TArgs1, TArgs2, Task> command, TArgs1 args1, TArgs2 args2)
        {
            _command = command;
            _args1 = args1;
            _args2 = args2;
        }

        protected override AsyncState CallStart()
        {
            return new AsyncStateTask(_command(_args1, _args2));
        }
    }
}
