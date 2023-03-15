using HereticalSolutions.Pools.AllocationCallbacks;

namespace HereticalSolutions.Pools.Factories
{
	public static partial class PoolsFactory
	{
		#region Allocation callbacks

		public static CompositeCallback<T> BuildCompositeCallback<T>(IAllocationCallback<T>[] callbacks)
		{
			return new CompositeCallback<T>(callbacks);
		}
		
		public static PushToPoolCallback<T> BuildPushToPoolCallback<T>(INonAllocPool<T> root = null)
		{
			return new PushToPoolCallback<T>(root);
		}
		
		public static PushToPoolCallback<T> BuildPushToPoolCallback<T>(DeferredCallbackQueue<T> deferredCallbackQueue)
		{
			return new PushToPoolCallback<T>(deferredCallbackQueue);
		}

		public static DeferredCallbackQueue<T> BuildDeferredCallbackQueue<T>()
		{
			return new DeferredCallbackQueue<T>();
		}

		#endregion
	}
}