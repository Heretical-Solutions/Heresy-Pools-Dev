namespace HereticalSolutions.Pools.AllocationCallbacks
{
    public class SetVariantCallback<T> : IAllocationCallback<T>
    {
        public int Variant { get; set; }

        public void OnAllocated(IPoolElement<T> currentElement)
        {
            if (currentElement.Value == null)
                return;

            ((VariantMetadata)currentElement.Metadata.Get<IContainsVariant>()).Variant = Variant;
        }
    }
}