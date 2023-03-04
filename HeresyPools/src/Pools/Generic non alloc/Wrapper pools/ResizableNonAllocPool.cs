using System;
using HereticalSolutions.Collections;
using HereticalSolutions.Collections.Allocations;
using HereticalSolutions.Pools.Behaviours;

namespace HereticalSolutions.Pools.GenricNonAlloc
{
	public class ResizableNonAllocPool<T>
		: INonAllocPool<T>,
		  IResizable<IPoolElement<T>>,
		  IModifiable<INonAllocPool<T>>,
		  ITopUppable<IPoolElement<T>>
	{
		private INonAllocPool<T> contents;
		private readonly IModifiable<IPoolElement<T>[]> contentsAsModifiable;
		private readonly IFixedSizeCollection<IPoolElement<T>> contentsAsFixedSizeCollection;

		private readonly IPushBehaviourHandler<T> pushBehaviourHandler;
		
		public ResizableNonAllocPool(
			INonAllocPool<T> contents,
			IModifiable<IPoolElement<T>[]> contentsAsModifiable,
			IFixedSizeCollection<IPoolElement<T>> contentsAsFixedSizeCollection,
			Action<ResizableNonAllocPool<T>> resizeDelegate,
			AllocationCommand<IPoolElement<T>> resizeAllocationCommand,
			Func<T> topUpAllocationDelegate)
		{
			this.contents = contents;
			
			this.contentsAsModifiable = contentsAsModifiable;
			
			this.contentsAsFixedSizeCollection = contentsAsFixedSizeCollection;
			
			this.resizeDelegate = resizeDelegate;

			this.topUpAllocationDelegate = topUpAllocationDelegate;

			ResizeAllocationCommand = resizeAllocationCommand;

			pushBehaviourHandler = new PushToINonAllocPoolBehaviour<T>(this);
		}
		
		#region IModifiable

		public INonAllocPool<T> Contents { get => contents; }
		
		public void UpdateContents(INonAllocPool<T> newContents)
		{
			contents = newContents;
		}
		
		public void UpdateCount(int newCount)
		{
			contentsAsModifiable.UpdateCount(newCount);
		}

		#endregion

		#region IResizable

		public AllocationCommand<IPoolElement<T>> ResizeAllocationCommand { get; private set; }

		protected Action<ResizableNonAllocPool<T>> resizeDelegate;

		public void Resize()
		{
			resizeDelegate(this);
		}

		#endregion

		#region ITopUppable

		private readonly Func<T> topUpAllocationDelegate;

		public void TopUp(IPoolElement<T> element)
		{
			element.Value = topUpAllocationDelegate.Invoke();
		}

		#endregion

		#region INonAllocPool

		public IPoolElement<T> Pop()
		{
			if (!contents.HasFreeSpace)
			{
				int previousCapacity = contentsAsFixedSizeCollection.Capacity;

				resizeDelegate(this);

				int newCapacity = contentsAsFixedSizeCollection.Capacity;
			}

			IPoolElement<T> result = contents.Pop();

			
			//Update element data
			var elementAsPushable = (IPushable<T>)result; 
            
			elementAsPushable.UpdatePushBehaviour(pushBehaviourHandler);
			
			
			return result;
		}

		public void Push(IPoolElement<T> instance)
		{
			contents.Push(instance);
		}
		
		public bool HasFreeSpace { get { return contents.HasFreeSpace; } }

		#endregion
	}
}