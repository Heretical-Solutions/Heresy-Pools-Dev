using System;

using HereticalSolutions.Collections.Allocations;

using HereticalSolutions.Pools.Allocations;

namespace HereticalSolutions.Pools.Factories
{
	public static partial class PoolsFactory
	{
		#region Non alloc pools

		public static INonAllocPool<T> BuildResizableNonAllocPool<T>(
					Func<T> valueAllocationDelegate,
					MetadataAllocationDescriptor[] metadataDescriptors,
					AllocationCommandDescriptor initialAllocation,
					AllocationCommandDescriptor additionalAllocation)
		{
			INonAllocPool<T> resizableNonAllocPool = BuildResizableNonAllocPool<T>(

				BuildPoolElementAllocationCommand<T>(
					initialAllocation,
					valueAllocationDelegate,
					metadataDescriptors),

				BuildPoolElementAllocationCommand<T>(
					additionalAllocation,
					valueAllocationDelegate,
					metadataDescriptors),

					valueAllocationDelegate);

			return resizableNonAllocPool;
		}

		#endregion
	}
}