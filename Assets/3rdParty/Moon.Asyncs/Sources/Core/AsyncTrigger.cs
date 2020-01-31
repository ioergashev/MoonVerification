using System.Collections.Generic;

namespace Moon.Asyncs
{
    public class AsyncTrigger
    {
        private readonly List<AsyncTriggerStatus> _triggers = new List<AsyncTriggerStatus>();

        public AsyncState AddTrigger()
        {
            var trigger = new AsyncTriggerStatus();
            _triggers.Add(trigger);
            return Planner.Chain()
                    .AddAwait(AwaitFunc, trigger)
                ;
        }

        private void AwaitFunc(AsyncTriggerStatus triggerStatus, AsyncStateInfo state)
        {
            if (triggerStatus == null) return;
            state.IsComplete = triggerStatus.complete;
        }

        public void Complete()
        {
            foreach (var trigger in _triggers)
            {
                trigger.complete = true;
            }
            _triggers.Clear();
        }
    }
}