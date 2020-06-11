using MiniGames.Common;
using Moon.Asyncs;
using UnityEngine;
using Zenject;

namespace MiniGames.Memory
{
    public class MemoryScenario : GameScenarioBase
    {
        public MemoryGameController controller;

        [Inject(Id = "camera_root")]
        private Animator camAnimator;

        //TODO: select gameModel with difficulty controller class
        public MemoryGameModel defaultGameModel;

        private RuntimeData runtimeData;

        private void Awake()
        {
            runtimeData = FindObjectOfType<RuntimeData>();
        }

        protected override AsyncState OnExecute()
        {
            return Planner.Chain()
                    .AddFunc(Intro)
                    .AddFunc(GameCircle)
                    .AddFunc(Outro)
                ;
        }

        private AsyncState Intro()
        {
            return Planner.Chain()
                    .AddAction(camAnimator.SetTrigger, "intro_trigger")
                    .AddTimeout(1f)
                ;
        }

        private AsyncState GameCircle()
        {
            var asyncChain = Planner.Chain();

            for (var i = 0; i < defaultGameModel.Cycles.Count; i++)
            {
                runtimeData.CycleIndex = i;
                runtimeData.CycleSettings = defaultGameModel.Cycles[i];

                asyncChain
                        .AddFunc(controller.RunGame, defaultGameModel)
                        .AddFunc(progress.IncrementProgress)
                    ;
            }

            return asyncChain;
        }

        private AsyncState Outro()
        {
            return Planner.Chain()
                    .AddAction(camAnimator.SetTrigger, "outro_trigger")
                    .AddTimeout(1f)
                ;
        }

    }
}