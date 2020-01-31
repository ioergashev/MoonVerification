namespace Moon.Asyncs.Internal
{
    public class AsyncChainInternal: AsyncChain
    {
        internal AsyncChainInternal(PlannerSynchronizationContext context) : base(context)
        {
        }

        internal void SetAsLoadingLayer()
        {
            SetLoadingLayer();
        }
    }
}
