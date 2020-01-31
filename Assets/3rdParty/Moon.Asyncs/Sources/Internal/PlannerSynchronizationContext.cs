using System.Collections.Generic;
using Moon.Asyncs.Internal.Commands;
using UnityEngine;

namespace Moon.Asyncs.Internal
{
    internal class PlannerSynchronizationContext
    {        
        private readonly List<AsyncChain> _chains = new List<AsyncChain>();
        private readonly ChainLayer[] _chainsLayers;

        internal PlannerSynchronizationContext()
        {
            _chainsLayers = new ChainLayer[(int)AsyncLayer.Game + 1];
            for (var i = 0; i < _chainsLayers.Length; i++)
            {
                _chainsLayers[i] = new ChainLayer();
            }
        }

        internal ChainLayer GetLayer(AsyncLayer layer)
        {
            return _chainsLayers[(int)layer];
        }

        internal void TerminateChains(AsyncLayer layer)
        {
            int count = _chains.Count;
            for (var i = 0; i < count; i++)
            {
                AsyncChain chain = _chains[i];
                if (layer.HasFlag(chain.layer))
                    chain.Terminate();
            }
        }

        internal AsyncChain Chain()
        {
            PlannerComponent.Initialize(this);
            var result = new AsyncChainInternal(this);
            _chains.Add(result);
            return result;
        }
        
        internal void Update()
        {
            for (var i = 0; i < _chains.Count; i++)
            {
                AsyncChain chain = _chains[i];
                ChainLayer layer = _chainsLayers[(int)chain.layer];
                if (layer.paused) continue;
                AsyncCommandResult updateResult = chain.UpdateChain();

                if (updateResult == AsyncCommandResult.Exception)
                {
                    Debug.Log($"Cain '{chain.id}' finished terminated with exception");
                }

                if (!chain.isComplete && updateResult == AsyncCommandResult.Ok) continue;
                chain.ReportComplete();
                _chainsLayers[(int)chain.layer].Dec();
                _chains.RemoveAt(i);
                i--;
            }
        }
    }
}
