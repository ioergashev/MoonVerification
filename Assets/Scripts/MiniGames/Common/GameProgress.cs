using Moon.Asyncs;
using UnityEngine;

namespace MiniGames.Common
{
    public class GameProgress : MonoBehaviour
    {
        public void ResetProgress(int count)
        {
            // TODO: reset progress to zero. Set progress max
        }

        public AsyncState IncrementProgress()
        {
            return Planner.Chain()
                    // TODO: run progress animation, await finish
                    .AddTimeout(1f)
                ;
        }
    }
}