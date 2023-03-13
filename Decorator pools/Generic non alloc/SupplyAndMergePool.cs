using System;

using HereticalSolutions.Collections;
using HereticalSolutions.Collections.Allocations;
using HereticalSolutions.Pools.Arguments;
using HereticalSolutions.Pools.Behaviours;

namespace HereticalSolutions.Pools.Decorators
{
	public class SupplyAndMergePool<T> 
		: INonAllocDecoratedPool<T>,
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

			pushBehaviourHandler = new PushToDecoratedPoolBehaviour<T>(this);
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

		#region INonAllocDecoratedPool
		
		public IPoolElement<T> Pop(IPoolDecoratorArgument[] args)
		{
			#region Append from argument
			
			if (args.TryGetArgument<AppendArgument>(out var arg))
			{
				var appendee = Append();

				#region Update push behaviour

				var appendeeElementAsPushable = (IPushable<T>)appendee; 
            
				appendeeElementAsPushable.UpdatePushBehaviour(pushBehaviourHandler);

				#endregion
				
				return appendee;
			}
			
			#endregion
			
			#region Top up and merge
			
			if (!basePool.HasFreeSpace)
			{
				TopUpAndMerge();
			}
			
			#endregion

			IPoolElement<T> result = basePool.Pop();

			#region Top up

			if (result.Value.Equals(default(T)))
			{
				TopUp(result);
			}
			
			#endregion
			
			#region Update push behaviour
			
			var elementAsPushable = (IPushable<T>)result; 
            
			elementAsPushable.UpdatePushBehaviour(pushBehaviourHandler);
			
			#endregion

			return result;
		}

		public void Push(
			IPoolElement<T> instance,
			bool decoratorsOnly = false)
		{
			if (decoratorsOnly)
				return;

			#region Top up and merge
			
			var instanceIndex = instance.Metadata.Get<IIndexed>().Index;

			if (instanceIndex > -1
			    && instanceIndex < supplyPoolAsIndexable.Count
			    && supplyPoolAsIndexable[instanceIndex] == instance)
			{
				TopUpAndMerge();
			}
			
			#endregion

			basePool.Push(instance);
		}
		
		public bool HasFreeSpace { get { return true; } }  // ¯\_(ツ)_/¯

		#endregion
	}
}