using System;
using DG.Tweening;
using Moon.Asyncs.Internal.Commands;

namespace Moon.Asyncs.Internal
{
    internal class AsyncCommandTweenArgs2<TArgs1, TArgs2> : AsyncCommand
    {
        private Func<TArgs1, TArgs2, Tween> _command;
        private TArgs1 _args1;
        private TArgs2 _args2;

        public AsyncCommandTweenArgs2(Func<TArgs1, TArgs2, Tween> command, TArgs1 args1, TArgs2 args2)
        {
            _command = command;
            _args1 = args1;
            _args2 = args2;
        }

        protected override AsyncState CallStart()
        {
            Tween tween = _command(_args1, _args2);
            return new AsyncStateTween(tween);
        }
    }
}