using System;
using HereticalSolutions.Collections.Allocations;
using HereticalSolutions.Pools.Allocations;
using HereticalSolutions.Pools.GenericNonAlloc;

namespace HereticalSolutions.Pools.Factories
{
	public static partial class PoolsFactory
	{
		#region Supply and merge pool

		public static INonAllocPool<T> BuildSupplyAndMergePool<T>(
			Func<T> valueAllocationDelegate,
			MetadataAllocationDescriptor[] metadataDescriptors,
			AllocationCommandDescriptor initialAllocation,
			AllocationCommandDescriptor additionalAllocation)
		{
			Func<T> nullAllocation = NullAllocationDelegate<T>;

			INonAllocPool<T> supplyAndMergePool = BuildSupplyAndMergePool<T>(

				BuildPoolElementAllocationCommand<T>(
					initialAllocation,
					valueAllocationDelegate,
					metadataDescriptors),

				BuildPoolElementAllocationCommand<T>(
					additionalAllocation,
					nullAllocation,
					metadataDescriptors),

				valueAllocationDelegate);

			return supplyAndMergePool;
		}
		
		public static SupplyAndMergePool<T> BuildSupplyAndMergePool<T>(
			AllocationCommand<IPoolElement<T>> initialAllocationCommand,
			AllocationCommand<IPoolElement<T>> appendAllocationCommand,
			Func<T> topUpAllocationDelegate)
		{
			var basePool = BuildPackedArrayPool<T>(initialAllocationCommand);

			var supplyPool = BuildPackedArrayPool<T>(appendAllocationCommand);

			return new SupplyAndMergePool<T>(
				basePool,
				supplyPool,
				supplyPool,
				supplyPool,
				appendAllocationCommand,
				MergePools,
				topUpAllocationDelegate);
		}

		public static void MergePools<T>(
			INonAllocPool<T> receiverArray,
			INonAllocPool<T> donorArray,
			AllocationCommand<IPoolElement<T>> donorAllocationCommand)
		{
			MergePackedArrayPools(
				(PackedArrayPool<T>)receiverArray,
				(PackedArrayPool<T>)donorArray,
				donorAllocationCommand);
		}

		#endregion
	}
}