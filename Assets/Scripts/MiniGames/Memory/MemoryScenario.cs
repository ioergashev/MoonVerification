﻿using MiniGames.Common;
using Moon.Asyncs;
using UnityEngine;

namespace MiniGames.Memory
{
    public class MemoryScenario : GameScenarioBase
    {
        public MemoryGameController controller;

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
                    // TODO: intro cut scene. Run camera animation. Await animation finish
                    .AddAction(Debug.Log, "start intro")
                    .AddTimeout(1f)
                    .AddAction(Debug.Log, "intro finished")
                ;
        }

        private AsyncState GameCircle()
        {
            var asyncChain = Planner.Chain();
            asyncChain.AddAction(Debug.Log, "game started");
            // TODO: implement game circle using game controller
            // TODO: move hardcoded "5" count to game config
            for (var i = 0; i < defaultGameModel.Cycles.Count; i++)
            {
                runtimeData.CycleIndex = i;
                runtimeData.CycleSettings = defaultGameModel.Cycles[i];

                asyncChain
                        .AddFunc(controller.RunGame, defaultGameModel)
                        .AddFunc(progress.IncrementProgress)
                    ;
            }

            asyncChain.AddAction(Debug.Log, "game finished");
            return asyncChain;
        }

        private AsyncState Outro()
        {
            // TODO: outro cut scene. Run camera back to origin position. Await animation finish
            return Planner.Chain()
                    .AddAction(Debug.Log, "start outro")
                    .AddTimeout(1f)
                    .AddAction(Debug.Log, "outro finished")
                ;
        }

    }
}