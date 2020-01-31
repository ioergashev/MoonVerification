using System.Collections.Generic;
using UnityEngine;

namespace Moon.Asyncs
{
    /// <summary>
    /// Queue for delayed events. You can use string Id or Unity object for event binding.
    /// </summary>
    public class EventAwaiters
    {
        private Dictionary<string, Queue<EventAwaiter>> _awaiters;
        
        private Dictionary<string, Queue<EventAwaiter>> GetAwaiters()
        {
            if (_awaiters == null)
            {
                _awaiters = new Dictionary<string, Queue<EventAwaiter>>();
            }
            return _awaiters;
        }

        /// <summary>
        /// Enqueue <see cref="EventAwaiter"/> with specific Id.
        /// You can await <see cref="EventAwaiter.State"/> and call <see cref="Invoke(string)"/> for continuation async chain.
        /// </summary>
        public EventAwaiter Enqueue(string eventId)
        {
            var awaiters = GetAwaiters();
            if (!awaiters.ContainsKey(eventId))
            {
                awaiters.Add(eventId, new Queue<EventAwaiter>());
            }
            var awaiter = awaiters[eventId];
            var result = new EventAwaiter();
            awaiter.Enqueue(result);
            return result;
        }

        /// <summary>
        /// Enqueue <see cref="EventAwaiter"/>. Use <see cref="Object.GetInstanceID"/> as event Id.
        /// For more information <see cref="Enqueue(string)"/>.
        /// </summary>
        public EventAwaiter Enqueue(Object unityObj)
        {
            return Enqueue(unityObj.GetInstanceID().ToString());
        }

        /// <summary>
        /// Find awaiter with given <paramref name="eventId"/> and complete it.
        /// </summary>
        public void Invoke(string eventId)
        {
            var awaiters = GetAwaiters();
            if (!awaiters.ContainsKey(eventId))
            {
                return;
            }
            var awaiter = awaiters[eventId];
            while (awaiter.Count > 0)
            {
                awaiter.Dequeue().Invoke();
            }
        }

        /// <summary>
        /// Find awaiter with given <paramref name="unityObj"/> and complete it.
        /// </summary>
        public void Invoke(Object unityObj)
        {
            Invoke(unityObj.GetInstanceID().ToString());
        }
    }
}
