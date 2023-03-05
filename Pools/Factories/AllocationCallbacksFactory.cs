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
		
		public static PushToPoolCallback<T> BuildPushToPoolCallback<T>()
		{
			return new PushToPoolCallback<T>();
		}

		#endregion
	}
}