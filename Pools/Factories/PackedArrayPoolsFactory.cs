using System;

using HereticalSolutions.Collections;
using HereticalSolutions.Collections.Allocations;

using HereticalSolutions.Pools.GenericNonAlloc;

namespace HereticalSolutions.Pools.Factories
{
	public static partial class PoolsFactory
	{
		#region Packed array pool

		#region Build
		
		public static PackedArrayPool<T> BuildPackedArrayPool<T>(
			AllocationCommand<IPoolElement<T>> allocationCommand)
		{
			int initialAmount = CountInitialAllocationAmount<T>(allocationCommand);

			IPoolElement<T>[] contents = new IPoolElement<T>[initialAmount];

			PerformAllocation(initialAmount, contents, allocationCommand);

			return new PackedArrayPool<T>(contents);
		}

		private static int CountInitialAllocationAmount<T>(AllocationCommand<IPoolElement<T>> allocationCommand)
		{
			int initialAmount = -1;

			switch (allocationCommand.Descriptor.Rule)
			{
				case EAllocationAmountRule.ZERO:
					initialAmount = 0;
					break;

				case EAllocationAmountRule.ADD_ONE:
					initialAmount = 1;
					break;

				case EAllocationAmountRule.ADD_PREDEFINED_AMOUNT:
					initialAmount = allocationCommand.Descriptor.Amount;
					break;

				default:
					throw new Exception($"[PoolsFactory] INVALID ALLOCATION COMMAND RULE: {allocationCommand.Descriptor.Rule.ToString()}");
			}

			return initialAmount;
		}

		private static void PerformAllocation<T>(
			int initialAmount,
			IPoolElement<T>[] contents,
			AllocationCommand<IPoolElement<T>> allocationCommand)
		{
			for (int i = 0; i < initialAmount; i++)
				contents[i] = allocationCommand.AllocationDelegate();
		}
		
		#endregion

		#region Resize
		
		public static void ResizePackedArrayPool<T>(
			PackedArrayPool<T> arrayPool,
			AllocationCommand<IPoolElement<T>> allocationCommand)
		{
			int newCapacity = CountResizeAllocationAmount(arrayPool, allocationCommand);

			IPoolElement<T>[] oldContents = ((IModifiable<IPoolElement<T>[]>)arrayPool).Contents;

			IPoolElement<T>[] newContents = new IPoolElement<T>[newCapacity];

			FillNewArrayWithContents(newCapacity, arrayPool, oldContents, newContents, allocationCommand);

			((IModifiable<IPoolElement<T>[]>)arrayPool).UpdateContents(newContents);
		}

		private static int CountResizeAllocationAmount<T>(
			PackedArrayPool<T> arrayPool,
			AllocationCommand<IPoolElement<T>> allocationCommand)
		{
			int newCapacity = -1;

			switch (allocationCommand.Descriptor.Rule)
			{
				case EAllocationAmountRule.ADD_ONE:
					newCapacity = arrayPool.Capacity + 1;
					break;

				case EAllocationAmountRule.DOUBLE_AMOUNT:
					newCapacity = Math.Max(arrayPool.Capacity, 1) * 2;
					break;

				case EAllocationAmountRule.ADD_PREDEFINED_AMOUNT:
					newCapacity = arrayPool.Capacity + allocationCommand.Descriptor.Amount;
					break;

				default:
					throw new Exception($"[PoolsFactory] INVALID ALLOCATION COMMAND RULE FOR INDEXED PACKED ARRAY: {allocationCommand.Descriptor.Rule.ToString()}");
			}

			return newCapacity;
		}

		private static void FillNewArrayWithContents<T>(
			int newCapacity,
			PackedArrayPool<T> arrayPool,
			IPoolElement<T>[] oldContents,
			IPoolElement<T>[] newContents,
			AllocationCommand<IPoolElement<T>> allocationCommand)
		{
			if (newCapacity <= arrayPool.Capacity)
			{
				for (int i = 0; i < newCapacity; i++)
					newContents[i] = oldContents[i];
			}
			else
			{
				for (int i = 0; i < arrayPool.Capacity; i++)
					newContents[i] = oldContents[i];

				for (int i = arrayPool.Capacity; i < newCapacity; i++)
					newContents[i] = allocationCommand.AllocationDelegate();
			}
		}

		#endregion

		#region Merge
		
		public static void MergePackedArrayPools<T>(
			PackedArrayPool<T> receiverArrayPool,
			PackedArrayPool<T> donorArrayPool,
			AllocationCommand<IPoolElement<T>> donorAllocationCommand)
		{
			UpdateReceiverContents(receiverArrayPool, donorArrayPool);

			UpdateDonorContents(donorArrayPool, donorAllocationCommand);
		}

		#region Update receiver contents

		private static void UpdateReceiverContents<T>(
			PackedArrayPool<T> receiverArrayPool,
			PackedArrayPool<T> donorArrayPool)
		{
			IPoolElement<T>[] oldReceiverContents = ((IModifiable<IPoolElement<T>[]>)receiverArrayPool).Contents;

			IPoolElement<T>[] oldDonorContents = ((IModifiable<IPoolElement<T>[]>)donorArrayPool).Contents;
			
			
			int newReceiverCapacity = receiverArrayPool.Capacity + donorArrayPool.Capacity;

			IPoolElement<T>[] newReceiverContents = new IPoolElement<T>[newReceiverCapacity];

			
			CopyOldReceiverContents(receiverArrayPool, oldReceiverContents, newReceiverContents);

			AppendOldDonorContents(receiverArrayPool, donorArrayPool, oldDonorContents, newReceiverContents);

			
			if (receiverArrayPool.Capacity == receiverArrayPool.Count)
			{
				UpdateIndexesOnElementsFromDonorArray(receiverArrayPool, donorArrayPool, newReceiverContents);
			}
			else
			{
				PackElementsFromDonorArray(receiverArrayPool, donorArrayPool, newReceiverContents);
			}

			
			((IModifiable<IPoolElement<T>[]>)receiverArrayPool).UpdateContents(newReceiverContents);

			((ICountUpdateable)receiverArrayPool).UpdateCount(receiverArrayPool.Count + donorArrayPool.Count);
		}

		private static void CopyOldReceiverContents<T>(
			PackedArrayPool<T> receiverArrayPool,
			IPoolElement<T>[] oldReceiverContents,
			IPoolElement<T>[] newReceiverContents)
		{
			for (int i = 0; i < receiverArrayPool.Capacity; i++)
				newReceiverContents[i] = oldReceiverContents[i];
		}

		private static void AppendOldDonorContents<T>(
			PackedArrayPool<T> receiverArrayPool,
			PackedArrayPool<T> donorArrayPool,
			IPoolElement<T>[] oldDonorContents,
			IPoolElement<T>[] newReceiverContents)
		{
			for (int i = 0; i < donorArrayPool.Capacity; i++)
			{
				int newIndex = i + receiverArrayPool.Capacity;

				newReceiverContents[newIndex] = oldDonorContents[i];
			}
		}

		private static void UpdateIndexesOnElementsFromDonorArray<T>(
			PackedArrayPool<T> receiverArrayPool,
			PackedArrayPool<T> donorArrayPool,
			IPoolElement<T>[] newReceiverContents)
		{
			for (int i = 0; i < donorArrayPool.Count; i++)
			{
				int newIndex = i + receiverArrayPool.Capacity;

				newReceiverContents[newIndex].Metadata.Get<IIndexed>().Index = newIndex;
			}
		}

		private static void PackElementsFromDonorArray<T>(
			PackedArrayPool<T> receiverArrayPool,
			PackedArrayPool<T> donorArrayPool,
			IPoolElement<T>[] newReceiverContents)
		{
			int lastReceiverFreeItemIndex = receiverArrayPool.Count;

			for (int i = 0; i < donorArrayPool.Count; i++)
			{
				int newIndex = i + receiverArrayPool.Capacity;

				
				newReceiverContents[lastReceiverFreeItemIndex].Metadata.Get<IIndexed>().Index = -1;

				newReceiverContents[newIndex].Metadata.Get<IIndexed>().Index = lastReceiverFreeItemIndex;


				var swap = newReceiverContents[newIndex];

				newReceiverContents[newIndex] = newReceiverContents[lastReceiverFreeItemIndex];

				newReceiverContents[lastReceiverFreeItemIndex] = swap;


				lastReceiverFreeItemIndex++;
			}
		}

		#endregion

		#region Update donor contents
		
		private static void UpdateDonorContents<T>(
			PackedArrayPool<T> donorArrayPool,
			AllocationCommand<IPoolElement<T>> donorAllocationCommand)
		{
			int newDonorCapacity = -1;

			switch (donorAllocationCommand.Descriptor.Rule)
			{
				case EAllocationAmountRule.ADD_ONE:
					newDonorCapacity = 1;
					break;

				case EAllocationAmountRule.ADD_PREDEFINED_AMOUNT:
					newDonorCapacity = donorAllocationCommand.Descriptor.Amount;
					break;

				default:
					throw new Exception($"[PoolsFactory] INVALID DONOR ALLOCATION COMMAND RULE: {donorAllocationCommand.Descriptor.Rule.ToString()}");
			}

			
			IPoolElement<T>[] newDonorContents = new IPoolElement<T>[newDonorCapacity];

			for (int i = 0; i < newDonorCapacity; i++)
				newDonorContents[i] = donorAllocationCommand.AllocationDelegate();

			
			((IModifiable<IPoolElement<T>[]>)donorArrayPool).UpdateContents(newDonorContents);

			((ICountUpdateable)donorArrayPool).UpdateCount(0);
		}

		#endregion
		
		#endregion

		#endregion
	}
}