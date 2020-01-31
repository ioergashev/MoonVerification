using System;
using Moon.Asyncs.Internal;

namespace Moon.Asyncs
{
    /// <summary>
    /// Scheduler for async commands queues
    /// </summary>
    public static class Planner
    {
        private static PlannerSynchronizationContext _context = new PlannerSynchronizationContext();

        /// <summary>
        /// Rise when starts chain marked as cut-scene
        /// </summary>
        public static event Action OnStartCutScene
        {
            add
            {
                var layer = _context.GetLayer(AsyncLayer.CutScene);
                layer.OnStart += value;
            }
            remove
            {
                var layer = _context.GetLayer(AsyncLayer.CutScene);
                layer.OnStart -= value;
            }
        }

        /// <summary>
        /// Rise when finish all chains marked as cut-scene
        /// </summary>
        public static event Action OnFinishCutScene
        {
            add
            {
                var layer = _context.GetLayer(AsyncLayer.CutScene);
                layer.OnFinish += value;
            }
            remove
            {
                var layer = _context.GetLayer(AsyncLayer.CutScene);
                layer.OnFinish -= value;
            }
        }

        /// <summary>
        /// Are chains marked as cut-scene paused,
        /// </summary>
        /// <returns>true if cut-scenes paused, false otherwise</returns>
        public static bool IsCutScenePause()
        {
            var layer = _context.GetLayer(AsyncLayer.CutScene);
            return layer.paused;
        }

        /// <summary>
        /// Pause chains marked as cut-scene
        /// </summary>
        public static void PauseCutScene()
        {
            var layer = _context.GetLayer(AsyncLayer.CutScene);
            layer.paused = true;
        }

        /// <summary>
        /// Resume chains marked as cut-scene
        /// </summary>
        public static void ResumeCutScene()
        {
            var layer = _context.GetLayer(AsyncLayer.CutScene);
            layer.paused = false;
        }

        /// <summary>
        /// Terminate chains marked as cut-scene
        /// </summary>
        public static void TerminateCutScene()
        {
            _context.TerminateChains(AsyncLayer.CutScene);
        }

        /// <summary>
        /// Terminate all chains
        /// </summary>
        public static void TerminateAll()
        {
            _context.TerminateChains(AsyncLayer.Game | AsyncLayer.CutScene);
        }

        /// <summary>
        /// Creates new commnads queue
        /// </summary>
        /// <returns>new commnads queue instance</returns>
        public static AsyncChain Chain()
        {
            return _context.Chain();
        }

        /// <summary>
        /// Creates new commnads queue with delay first command
        /// </summary>
        /// <param name="t">Seconds to wait before next command</param>
        /// <returns>new commnads queue instance</returns>
        internal static AsyncChain Timeout(float t)
        {
            return Chain().AddTimeout(t);
        }

        /// <summary>
        /// Creates new commnads queue with empty first command
        /// </summary>
        /// <returns>new commnads queue instance</returns>
        internal static AsyncChain Empty()
        {
            return Chain().AddEmpty();
        }
    }
}
