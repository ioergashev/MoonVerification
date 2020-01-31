using System.Collections;
using UnityEngine;

namespace Moon.Asyncs
{
    public static class CoroutinesExtension
    {
        public static AsyncChain AsAsync(this IEnumerator coroutine)
        {
            return Planner.Chain().AddCoroutine(coroutine);
        }
        public static AsyncChain AsAsync(this WaitForEndOfFrame coroutine)
        {
            return Planner.Chain().AddCoroutine(coroutine);
        }
        public static AsyncChain AsAsync(this AsyncOperation coroutine)
        {
            return Planner.Chain().AddCoroutine(coroutine);
        }

    }
}
