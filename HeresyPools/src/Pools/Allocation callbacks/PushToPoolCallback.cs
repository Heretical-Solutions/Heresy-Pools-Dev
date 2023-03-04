namespace HereticalSolutions.Pools.AllocationCallbacks
{
    public class PushToPoolCallback<T> : IAllocationCallback<T>
    {
        public INonAllocPool<T> Root { get; set; }

        public void OnAllocated(IPoolElement<T> currentElement)
        {
            if (currentElement.Value == null)
                return;

            Root.Push(currentElement);
        }
    }
}