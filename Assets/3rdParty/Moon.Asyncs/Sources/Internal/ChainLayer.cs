using System;

namespace Moon.Asyncs.Internal
{
    internal class ChainLayer
    {
        private int _chainsCount;

        public bool paused;

        public event Action OnStart;
        public event Action OnFinish;

        private void Start()
        {
            OnStart?.Invoke();
        }

        private void Finish()
        {
            OnFinish?.Invoke();
        }

        public void Inc()
        {
            var prevValue = _chainsCount;
            _chainsCount++;
            CountChanged(prevValue);
        }

        public void Dec()
        {
            var prevValue = _chainsCount;
            _chainsCount--;
            CountChanged(prevValue);
        }

        private void CountChanged(int prevValue)
        {
            if (prevValue == 0 && _chainsCount == 1)
            {
                OnStart?.Invoke();
            }
            if (prevValue == 1 && _chainsCount == 0)
            {
                OnFinish?.Invoke();
            }
        }
    }
}
