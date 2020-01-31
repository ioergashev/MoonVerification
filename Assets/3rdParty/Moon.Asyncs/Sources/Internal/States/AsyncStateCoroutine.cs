using System.Collections;
using UnityEngine;

namespace Moon.Asyncs.Internal
{
    internal class AsyncStateCoroutine : AsyncState
    {
        private IEnumerator _enumerator;

        public AsyncStateCoroutine(IEnumerator value)
        {
            _enumerator = value;
        }

        public AsyncStateCoroutine(WaitForEndOfFrame value)
        {
            _enumerator = YieldToEnumerable(value).GetEnumerator();
        }

        public AsyncStateCoroutine(AsyncOperation value)
        {
            _enumerator = YieldToEnumerable(value).GetEnumerator();
        }

        public override void Update()
        {
            if (_enumerator == null)
            {
                isComplete = true;
                return;
            }

            bool result = _enumerator.MoveNext();
            if (result) return;
            isComplete = true;
        }

        public override void Terminate()
        {
            _enumerator = null;
        }

        private static IEnumerable YieldToEnumerable(WaitForEndOfFrame value)
        {
            yield return value;
        }

        private static IEnumerable YieldToEnumerable(AsyncOperation value)
        {
            while (!value.isDone)
            {
                yield return 0;
            }
        }

    }
}