namespace Moon.Asyncs.Internal
{
    public static class AsyncChainInternalExtensions
    {
        public static AsyncChain AsLoading(this AsyncChain chain)
        {
            var internalChain = chain as AsyncChainInternal;
            internalChain?.SetAsLoadingLayer();
            return chain;
        }
    }
}
