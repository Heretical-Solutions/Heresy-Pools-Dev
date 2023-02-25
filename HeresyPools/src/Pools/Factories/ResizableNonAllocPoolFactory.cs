using System;

using HereticalSolutions.Collections.Allocations;
using HereticalSolutions.Pools.GenericNonAlloc;
using HereticalSolutions.Pools.GenricNonAlloc;

namespace HereticalSolutions.Pools.Factories
{
	public static partial class PoolsFactory
	{
		#region Resizable non alloc pool

		public static ResizableNonAllocPool<T> BuildResizableNonAllocPool<T>(
			AllocationCommand<IPoolElement<T>> initialAllocationCommand,
			AllocationCommand<IPoolElement<T>> resizeAllocationCommand,
			Func<T> topUpAllocationDelegate)
		{
			var pool = BuildPackedArrayPool<T>(initialAllocationCommand);

			return new ResizableNonAllocPool<T>(
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