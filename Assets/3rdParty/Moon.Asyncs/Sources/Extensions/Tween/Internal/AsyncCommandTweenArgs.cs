using System;
using DG.Tweening;
using Moon.Asyncs.Internal.Commands;

namespace Moon.Asyncs.Internal
{
    internal class AsyncCommandTweenArgs<TArgs> : AsyncCommand
    {
        private Func<TArgs, Tween> _command;
        private TArgs _args;

        public AsyncCommandTweenArgs(Func<TArgs, Tween> command, TArgs args)
        {
            _command = command;
            _args = args;
        }

        protected override AsyncState CallStart()
        {
            Tween tween = _command(_args);
            return new AsyncStateTween(tween);
        }
    }
}