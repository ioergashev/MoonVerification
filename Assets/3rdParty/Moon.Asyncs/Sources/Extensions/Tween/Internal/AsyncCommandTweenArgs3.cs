using System;
using DG.Tweening;
using Moon.Asyncs.Internal.Commands;

namespace Moon.Asyncs.Internal
{
    internal class AsyncCommandTweenArgs3<TArgs1, TArgs2, TArgs3> : AsyncCommand
    {
        private Func<TArgs1, TArgs2, TArgs3, Tween> _command;
        private TArgs1 _args1;
        private TArgs2 _args2;
        private TArgs3 _args3;

        public AsyncCommandTweenArgs3(Func<TArgs1, TArgs2, TArgs3, Tween> command, TArgs1 args1, TArgs2 args2, TArgs3 args3)
        {
            _command = command;
            _args1 = args1;
            _args2 = args2;
            _args3 = args3;
        }

        protected override AsyncState CallStart()
        {
            Tween tween = _command(_args1, _args2, _args3);
            return new AsyncStateTween(tween);
        }
    }
}