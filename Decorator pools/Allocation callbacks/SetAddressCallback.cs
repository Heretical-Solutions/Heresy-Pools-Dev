namespace HereticalSolutions.Pools.AllocationCallbacks
{
    public class SetAddressCallback<T> : IAllocationCallback<T>
    {
        public string FullAddress { get; set; }
        public int[] AddressHashes { get; set; }

        public void OnAllocated(IPoolElement<T> currentElement)
        {
            if (currentElement.Value == null)
                return;

            var addressMetadata = (AddressMetadata)currentElement.Metadata.Get<IContainsAddress>();

            addressMetadata.FullAddress = FullAddress;
            addressMetadata.AddressHashes = AddressHashes;
        }
    }
}