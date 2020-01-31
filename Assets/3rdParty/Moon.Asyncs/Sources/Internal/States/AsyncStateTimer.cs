using UnityEngine;

namespace Moon.Asyncs.Internal
{
    internal class AsyncStateTimer : AsyncState
    {
        float _time;
        private bool _independent;
        public AsyncStateTimer(float time, bool independent)
        {
            _independent = independent;
            _time = time;
        }

        public override void Terminate()
        {
            _time = 0f;
        }

        public override void Update()
        {
            var delta = Time.deltaTime;
            if (_independent)
            {
                delta = Time.unscaledDeltaTime;
            }
            _time -= delta;
            isComplete = _time <= 0f;
        }
    }

}
