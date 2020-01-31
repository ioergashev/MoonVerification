using System;
using DG.Tweening;
using Moon.Asyncs.Internal.Commands;

namespace Moon.Asyncs.Internal
{
    internal class AsyncCommandTween : AsyncCommand
    {
        private Func<Tween> _command;

        public AsyncCommandTween(Func<Tween> command)
        {
            _command = command;
        }

        protected override AsyncState CallStart()
        {
            Tween tween = _command();
            return new AsyncStateTween(tween);
        }
    }
}