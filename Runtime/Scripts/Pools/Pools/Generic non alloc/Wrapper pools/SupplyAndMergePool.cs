using System;

using HereticalSolutions.Collections;
using HereticalSolutions.Collections.Allocations;

using HereticalSolutions.Pools.Behaviours;

namespace HereticalSolutions.Pools.GenericNonAlloc
{
	public class SupplyAndMergePool<T> 
		: INonAllocPool<T>,
		  IAppendable<IPoolElement<T>>,
		  ITopUppable<IPoolElement<T>>
	{
		private readonly INonAllocPool<T> basePool;

		private readonly INonAllocPool<T> supplyPool;
		private readonly IIndexable<IPoolElement<T>> supplyPoolAsIndexable;
		private readonly IFixedSizeCollection<IPoolElement<T>> supplyPoolAsFixedSizeCollection;
		
		private readonly IPushBehaviourHandler<T> pushBehaviourHandler;

		public SupplyAndMergePool(
			INonAllocPool<T> basePool,
			INonAllocPool<T> supplyPool,
			IIndexable<IPoolElement<T>> supplyPoolAsIndexable,
			IFixedSizeCollection<IPoolElement<T>> supplyPoolAsFixedSizeCollection,
			AllocationCommand<IPoolElement<T>> appendAllocationCommand,
			Action<INonAllocPool<T>, INonAllocPool<T>, AllocationCommand<IPoolElement<T>>> mergeDelegate,
			Func<T> topUpAllocationDelegate)
		{
			this.basePool = basePool;

			this.supplyPool = supplyPool;
			this.supplyPoolAsIndexable = supplyPoolAsIndexable;
			this.supplyPoolAsFixedSizeCollection = supplyPoolAsFixedSizeCollection;
			
			this.mergeDelegate = mergeDelegate;

			this.topUpAllocationDelegate = topUpAllocationDelegate;

			AppendAllocationCommand = appendAllocationCommand;

			pushBehaviourHandler = new PushToINonAllocPoolBehaviour<T>(this);
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

		#endregion

		#region ITopUppable

		private readonly Func<T> topUpAllocationDelegate;

		public void TopUp(IPoolElement<T> element)
		{
			element.Value = topUpAllocationDelegate.Invoke();
		}

		#endregion

		#region Merge

		private readonly Action<INonAllocPool<T>, INonAllocPool<T>, AllocationCommand<IPoolElement<T>>> mergeDelegate;

		private void MergeSupplyIntoBase()
		{
			mergeDelegate.Invoke(basePool, supplyPool, AppendAllocationCommand);
		}

		private void TopUpAndMerge()
		{
			for (int i = supplyPoolAsIndexable.Count; i < supplyPoolAsFixedSizeCollection.Capacity; i++)
				TopUp(supplyPoolAsFixedSizeCollection.ElementAt(i));

			MergeSupplyIntoBase();
		}

		#endregion

		#region INonAllocPool
		
		public IPoolElement<T> Pop()
		{
			if (!basePool.HasFreeSpace)
			{
				TopUpAndMerge();
			}

			IPoolElement<T> result = basePool.Pop();

			
			//Update element data
			var elementAsPushable = (IPushable<T>)result; 
            
			elementAsPushable.UpdatePushBehaviour(pushBehaviourHandler);
			
			
			return result;
		}

		public void Push(IPoolElement<T> instance)
		{
			var instanceIndex = instance.Metadata.Get<IIndexed>().Index;

			if (instanceIndex > -1
			    && instanceIndex < supplyPoolAsIndexable.Count
			    && supplyPoolAsIndexable[instanceIndex] == instance)
			{
				TopUpAndMerge();
			}

			basePool.Push(instance);
		}
		
		public bool HasFreeSpace { get { return true; } }  // ¯\_(ツ)_/¯

		#endregion
	}
}