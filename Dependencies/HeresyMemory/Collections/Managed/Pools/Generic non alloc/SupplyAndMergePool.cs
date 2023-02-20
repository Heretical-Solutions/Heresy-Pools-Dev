using System;

using HereticalSolutions.Collections.Allocations;

namespace HereticalSolutions.Collections.Managed
{
	public class SupplyAndMergePool<T> 
		: INonAllocPool<T>,
		  IAppendable<IPoolElement<T>>,
		  ITopUppable<T>
	{
		//protected IndexedPackedArray<T> baseArray;
		protected INonAllocPool<T> basePool;

		//protected IndexedPackedArray<T> supplyArray;
		protected INonAllocPool<T> supplyPool;
		protected IIndexable<IPoolElement<T>> supplyPoolAsIndexable;
		protected IFixedSizeCollection<IPoolElement<T>> supplyPoolAsFixedSizeCollection;

		public SupplyAndMergePool(
			//IndexedPackedArray<T> baseArray,
			INonAllocPool<T> basePool,
			//IndexedPackedArray<T> supplyArray,
			INonAllocPool<T> supplyPool,
			AllocationCommand<IPoolElement<T>> appendAllocationCommand,
			//Action<IndexedPackedArray<T>, IndexedPackedArray<T>, AllocationCommand<IPoolElement<T>>> mergeDelegate,
			Action<INonAllocPool<T>, INonAllocPool<T>, AllocationCommand<IPoolElement<T>>> mergeDelegate,
			Func<T> topUpAllocationDelegate)
		{
			//this.baseArray = baseArray;
			this.basePool = basePool;

			//this.supplyArray = supplyArray;
			this.supplyPool = supplyPool;
			supplyPoolAsIndexable = (IIndexable<IPoolElement<T>>)supplyPool;

			this.mergeDelegate = mergeDelegate;

			this.topUpAllocationDelegate = topUpAllocationDelegate;

			AppendAllocationCommand = appendAllocationCommand;
		}

		#region IAppendable

		public AllocationCommand<IPoolElement<T>> AppendAllocationCommand { get; private set; }

		public IPoolElement<T> Append()
		{
			if (!supplyPool.HasFreeSpace)
			{
				MergeSupplyIntoBase();
			}

			IPoolElement<T> result = supplyPool.Pop();

			return result;
		}
		
		/*
		public IPoolElement<T> Append()
		{
			if (!supplyArray.HasFreeSpace)
			{
				MergeSupplyIntoBase();
			}

			IPoolElement<T> result = supplyArray.Pop();

			return result;
		}
		*/

		#endregion

		#region ITopUppable

		private Func<T> topUpAllocationDelegate;

		public void TopUp(IPoolElement<T> element)
		{
			element.Value = topUpAllocationDelegate.Invoke();
		}

		#endregion

		#region Merge

		protected Action<INonAllocPool<T>, INonAllocPool<T>, AllocationCommand<IPoolElement<T>>> mergeDelegate;
		//protected Action<IndexedPackedArray<T>, IndexedPackedArray<T>, AllocationCommand<IPoolElement<T>>> mergeDelegate;

		protected void MergeSupplyIntoBase()
		{
			mergeDelegate.Invoke(basePool, supplyPool, AppendAllocationCommand);
		}

		public void TopUpAndMerge()
		{
			for (int i = supplyPoolAsIndexable.Count; i < supplyPoolAsFixedSizeCollection.Capacity; i++)
				TopUp(supplyPoolAsFixedSizeCollection.ElementAt(i));

			MergeSupplyIntoBase();
		}
		
		/*
		protected void MergeSupplyIntoBase()
		{
			mergeDelegate.Invoke(baseArray, supplyArray, AppendAllocationCommand);
		}

		public void TopUpAndMerge()
		{
			for (int i = supplyArray.Count; i < supplyArray.Capacity; i++)
				TopUp(supplyArray.ElementAt(i));

			MergeSupplyIntoBase();
		}
		*/

		#endregion

		#region INonAllocPool
		
		public IPoolElement<T> Pop()
		{
			if (!basePool.HasFreeSpace)
			{
				TopUpAndMerge();
			}

			IPoolElement<T> result = basePool.Pop();

			return result;
		}

		public void Push(IPoolElement<T> instance)
		{
			var instanceIndex = ((IIndexed)instance).Index;

			if (instanceIndex > -1
			    && instanceIndex < supplyPoolAsIndexable.Count
			    && supplyPoolAsIndexable[instanceIndex] == instance)
			{
				TopUpAndMerge();
			}

			basePool.Push(instance);
		}
		
		/*
		public IPoolElement<T> Pop()
		{
			if (!baseArray.HasFreeSpace)
			{
				TopUpAndMerge();
			}

			IPoolElement<T> result = baseArray.Pop();

			return result;
		}

		public void Push(IPoolElement<T> instance)
		{
			var instanceIndex = ((IIndexed)instance).Index;

			if (instanceIndex > -1
				&& instanceIndex < supplyArray.Count
				&& supplyArray[instanceIndex] == instance)
			{
				TopUpAndMerge();
			}

			baseArray.Push(instance);
		}
		*/
		
		public bool HasFreeSpace { get { return true; } }  // ¯\_(ツ)_/¯

		#endregion
	}
}