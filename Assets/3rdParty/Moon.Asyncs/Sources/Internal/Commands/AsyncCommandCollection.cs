using System;
using System.Collections.Generic;

namespace Moon.Asyncs.Internal.Commands
{
    internal class AsyncCommandCollection
    {
        public readonly List<AsyncCommand> commands = new List<AsyncCommand>();
        public bool isComplete => commands.Count == 0;

        public AsyncCommandCollection(AsyncCommand command)
        {
            commands.Add(command);
        }

        public AsyncCommandResult Start()
        {
            var result = AsyncCommandResult.Ok;
            for (var i = 0; i < commands.Count; i++)
            {
                try
                {
                    commands[i].Start();
                }
                catch (Exception ex)
                {
                    result = AsyncCommandResult.Exception;
                    UnityEngine.Debug.LogException(ex);
                    commands.RemoveAt(i);
                    i--;
                }
            }

            return result;
        }

        public void CheckNullStates()
        {
            for (var i = 0; i < commands.Count; i++)
            {
                if (commands[i].state != null) continue;
                commands.RemoveAt(i);
                i--;
            }
        }

        internal AsyncCommandResult UpdateStates()
        {
            var result = AsyncCommandResult.Ok;
            for (var i = 0; i < commands.Count; i++)
            {
                try
                {
                    commands[i].state.Update();
                }
                catch (Exception ex)
                {
                    result = AsyncCommandResult.Exception;
                    UnityEngine.Debug.LogError(ex.ToString());
                    commands.RemoveAt(i);
                    i--;
                }
            }

            return result;
        }

        public void CheckAndReportComplete()
        {
            for (var i = 0; i < commands.Count; i++)
            {
                var command = commands[i];
                if (command.state.isComplete)
                {
                    command.state.ReportComplete();
                    commands.RemoveAt(i);
                    i--;
                }
            }
        }

        internal void Terminate()
        {
            for (var i = 0; i < commands.Count; i++)
            {
                commands[i].Terminate();
            }
            commands.Clear();
        }

        internal void ReportChainTerminated()
        {
            for (var i = 0; i < commands.Count; i++)
            {
                commands[i].ReportChainTerminated();
            }
        }
    }

}
