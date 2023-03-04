using System;

using HereticalSolutions.Collections;
using HereticalSolutions.Collections.Allocations;

using HereticalSolutions.Pools.Behaviours;

namespace HereticalSolutions.Pools.GenericNonAlloc
{
	public class ResizableNonAllocPool<T>
		: INonAllocPool<T>,
		  IResizable<IPoolElement<T>>,
		  IModifiable<INonAllocPool<T>>,
		  ITopUppable<IPoolElement<T>>,
		  ICountUpdateable
	{
		private INonAllocPool<T> contents;
		private readonly ICountUpdateable contentsAsCountUpdateable;

		private readonly IPushBehaviourHandler<T> pushBehaviourHandler;
		
		public ResizableNonAllocPool(
			INonAllocPool<T> contents,
			ICountUpdateable contentsAsCountUpdateable,
			Action<ResizableNonAllocPool<T>> resizeDelegate,
			AllocationCommand<IPoolElement<T>> resizeAllocationCommand,
			Func<T> topUpAllocationDelegate)
		{
			this.contents = contents;
			
			this.contentsAsCountUpdateable = contentsAsCountUpdateable;
			
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
			contentsAsCountUpdateable.UpdateCount(newCount);
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
				resizeDelegate(this);
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