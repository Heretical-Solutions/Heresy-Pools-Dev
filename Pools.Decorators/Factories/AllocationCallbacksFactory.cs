using HereticalSolutions.Pools.AllocationCallbacks;

namespace HereticalSolutions.Pools.Factories
{
    public static partial class PoolsFactory
    {
        #region Allocation callbacks

        public static PushToDecoratedPoolCallback<T> BuildPushToDecoratedPoolCallback<T>(INonAllocDecoratedPool<T> root = null)
        {
            return new PushToDecoratedPoolCallback<T>(root);
        }
        
        public static PushToDecoratedPoolCallback<T> BuildPushToDecoratedPoolCallback<T>(DeferredCallbackQueue<T> deferredCallbackQueue)
        {
            return new PushToDecoratedPoolCallback<T>(deferredCallbackQueue);
        }

        public static SetAddressCallback<T> BuildSetAddressCallback<T>(
            string fullAddress = null,
            int[] addressHashes = null)
        {
            return new SetAddressCallback<T>(fullAddress, addressHashes);
        }

        public static SetVariantCallback<T> BuildSetVariantCallback<T>(int variant = -1)
        {
            return new SetVariantCallback<T>(variant);
        }

        #endregion
    }
}