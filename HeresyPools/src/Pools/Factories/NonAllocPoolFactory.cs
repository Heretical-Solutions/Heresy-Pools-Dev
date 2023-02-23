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
			INonAllocPool<T> packedArrayPool = BuildResizableNonAllocPool<T>(

				BuildPoolElementAllocationCommand<T>(
					initialAllocation,
					valueAllocationDelegate,
					containerAllocationDelegate),

				BuildPoolElementAllocationCommand<T>(
					additionalAllocation,
					valueAllocationDelegate,
					containerAllocationDelegate),

					valueAllocationDelegate);

			return packedArrayPool;
		}

		public static INonAllocPool<T> BuildSupplyAndMergePool<T>(
			Func<T> valueAllocationDelegate,
			Func<Func<T>, IPoolElement<T>> containerAllocationDelegate,
			AllocationCommandDescriptor initialAllocation,
			AllocationCommandDescriptor additionalAllocation)
		{
			Func<T> nullAllocation = NullAllocationDelegate<T>;

			INonAllocPool<T> supplyAndMergePool = BuildSupplyAndMergePool<T>(

				BuildPoolElementAllocationCommand<T>(
					initialAllocation,
					valueAllocationDelegate,
					containerAllocationDelegate),

				BuildPoolElementAllocationCommand<T>(
					additionalAllocation,
					nullAllocation,
					containerAllocationDelegate),

				valueAllocationDelegate);

			return supplyAndMergePool;
		}

		#endregion
	}
}