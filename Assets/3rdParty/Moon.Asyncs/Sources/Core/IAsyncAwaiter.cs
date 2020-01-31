using System.Runtime.CompilerServices;

namespace Moon.Asyncs
{
    /// <summary>
    /// Base interface for awaters
    /// </summary>
    /// <typeparam name="T">Type of async command result</typeparam>
    public interface IAsyncAwaiter<out T> : INotifyCompletion
    {
        bool IsCompleted { get; }
        T GetResult();
    }
}