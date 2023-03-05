using System;

using HereticalSolutions.Collections.Allocations;

using HereticalSolutions.Pools.Allocations;
using HereticalSolutions.Pools.GenericNonAlloc;

namespace HereticalSolutions.Pools.Factories
{
	public static partial class PoolsFactory
	{
		#region Resizable non alloc pool

		public static ResizableNonAllocPool<T> BuildResizableNonAllocPoolWithAllocationCallback<T>(
			Func<T> valueAllocationDelegate,
			MetadataAllocationDescriptor[] metadataDescriptors,
			AllocationCommandDescriptor initialAllocation,
			AllocationCommandDescriptor additionalAllocation,
			IAllocationCallback<T> callback)
		{
			ResizableNonAllocPool<T> resizableNonAllocPool = BuildResizableNonAllocPoolFromPackedArrayPool<T>(

				BuildPoolElementAllocationCommandWithCallback<T>(
					initialAllocation,
					valueAllocationDelegate,
					metadataDescriptors,
					callback),

				BuildPoolElementAllocationCommandWithCallback<T>(
					additionalAllocation,
					valueAllocationDelegate,
					metadataDescriptors,
					callback),

				valueAllocationDelegate);

			return resizableNonAllocPool;
		}
		
		public static ResizableNonAllocPool<T> BuildResizableNonAllocPool<T>(
			Func<T> valueAllocationDelegate,
			MetadataAllocationDescriptor[] metadataDescriptors,
			AllocationCommandDescriptor initialAllocation,
			AllocationCommandDescriptor additionalAllocation)
		{
			ResizableNonAllocPool<T> resizableNonAllocPool = BuildResizableNonAllocPoolFromPackedArrayPool<T>(

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
		
		public static ResizableNonAllocPool<T> BuildResizableNonAllocPoolFromPackedArrayPool<T>(
			AllocationCommand<IPoolElement<T>> initialAllocationCommand,
			AllocationCommand<IPoolElement<T>> resizeAllocationCommand,
			Func<T> topUpAllocationDelegate)
		{
			var pool = BuildPackedArrayPool<T>(initialAllocationCommand);

			return new ResizableNonAllocPool<T>(
				pool,
				pool,
				ResizeNonAllocPool,
				resizeAllocationCommand,
				topUpAllocationDelegate);
		}

		public static void ResizeNonAllocPool<T>(
			ResizableNonAllocPool<T> pool)
		{
			ResizePackedArrayPool(
				(PackedArrayPool<T>)pool.Contents,
				pool.ResizeAllocationCommand);
		}

		#endregion
	}
}