using System;

namespace Moon.Asyncs
{
    /// <summary>
    /// Attributes for commands in async chain
    /// </summary>
    [Flags]
    public enum CommandAttributes
    {
        /// <summary>
        /// Default command, no additional behaviour
        /// </summary>
        None,
        /// <summary>
        /// Force invoke command when chain terminated
        /// </summary>
        InvokeOnTerminate
    }

}
