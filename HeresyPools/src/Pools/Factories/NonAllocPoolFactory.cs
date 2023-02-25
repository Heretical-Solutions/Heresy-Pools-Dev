using System;

using HereticalSolutions.Collections.Allocations;

namespace HereticalSolutions.Pools.Factories
{
	public static partial class PoolsFactory
	{
		#region Non alloc pools

		public static INonAllocPool<T> BuildResizableNonAllocPool<T>(
					Func<T> valueAllocationDelegate,
					Func<Func<T>, IPoolElement<T>> containerAllocationDelegate,
					AllocationCommandDescriptor initialAllocation,
					AllocationCommandDescriptor additionalAllocation)
		{
			INonAllocPool<T> resizableNonAllocPool = BuildResizableNonAllocPool<T>(

				BuildPoolElementAllocationCommand<T>(
					initialAllocation,
					valueAllocationDelegate,
					containerAllocationDelegate),

				BuildPoolElementAllocationCommand<T>(
					additionalAllocation,
					valueAllocationDelegate,
					containerAllocationDelegate),

					valueAllocationDelegate);

			return resizableNonAllocPool;
		}

		#endregion
	}
}