using System;
using System.Threading.Tasks;

namespace Moon.Asyncs.Internal.Commands
{
    internal class AsyncCommandTaskArgs3<TArgs1, TArgs2, TArgs3> : AsyncCommand
    {
        private TArgs1 _args1;
        private TArgs2 _args2;
        private TArgs3 _args3;
        private Func<TArgs1, TArgs2, TArgs3, Task> _command;

        public AsyncCommandTaskArgs3(Func<TArgs1, TArgs2, TArgs3, Task> command, TArgs1 args1, TArgs2 args2, TArgs3 args3)
        {
            _command = command;
            _args1 = args1;
            _args2 = args2;
            _args3 = args3;
        }

        protected override AsyncState CallStart()
        {
            return new AsyncStateTask(_command(_args1, _args2, _args3));
        }
    }
}
