using System;

namespace Moon.Asyncs.Internal.Commands
{

    internal abstract class AsyncCommand
    {
        public AsyncState state;
        protected CommandAttributes attributes;
        public void Start()
        {
            state = CallStart() ?? Planner.Empty();
            state.Start();
        }

        public void Terminate()
        {
            state?.Terminate();
        }

        public virtual void ReportChainTerminated()
        {
            if ((attributes & CommandAttributes.InvokeOnTerminate) == CommandAttributes.None)
            {
                state?.ReportTerminate();
                return;
            }
            try
            {
                CallStart();
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.LogError(ex.ToString());
            }
        }

        protected abstract AsyncState CallStart();
    }

}
