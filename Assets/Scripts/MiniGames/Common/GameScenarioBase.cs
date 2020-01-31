using Moon.Asyncs;
using UnityEngine;

namespace MiniGames.Common
{
    public abstract class GameScenarioBase: MonoBehaviour
    {
        protected GameProgress progress => _progress;

        private GameProgress _progress;

        public void Inject(GameProgress gameProgress)
        {
            _progress = gameProgress;
        }

        public AsyncState ExecuteScenario()
        {
            return OnExecute();
        }

        protected abstract AsyncState OnExecute();

    }
}