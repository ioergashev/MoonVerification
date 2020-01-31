namespace Moon.Asyncs
{
    /// <summary>
    /// class-parameter for await custom action in async chain
    /// </summary>
    public class AsyncStateInfo
    {
        /// <summary>
        /// Set it flag true, when action complete
        /// </summary>
        public bool IsComplete;

        /// <summary>
        /// True if chain terminated, False otherwise
        /// </summary>
        public bool IsTerminated;
    }
}
