using System;
using HereticalSolutions.Collections;
using HereticalSolutions.Collections.Allocations;

namespace HereticalSolutions.Pools.GenricNonAlloc
{
	public class ResizableNonAllocPool<T>
		: INonAllocPool<T>,
		  IResizable<IPoolElement<T>>, //used by CollectionFactory to resize
		  IModifiable<INonAllocPool<T>>, //used by CollectionFactory to resize
		  ITopUppable<IPoolElement<T>>
	{
		protected INonAllocPool<T> nonAllocPool;
		protected IModifiable<IPoolElement<T>[]> poolAsModifiable;
		protected IFixedSizeCollection<IPoolElement<T>> poolAsFixedSizeCollection;

		public ResizableNonAllocPool(
			INonAllocPool<T> nonAllocPool,
			Action<ResizableNonAllocPool<T>> resizeDelegate,
			AllocationCommand<IPoolElement<T>> resizeAllocationCommand,
			Func<T> topUpAllocationDelegate)
		{
			this.nonAllocPool = nonAllocPool;
			poolAsModifiable = (IModifiable<IPoolElement<T>[]>)nonAllocPool;
			poolAsFixedSizeCollection = (IFixedSizeCollection<IPoolElement<T>>)nonAllocPool;

			this.resizeDelegate = resizeDelegate;

			this.topUpAllocationDelegate = topUpAllocationDelegate;

			ResizeAllocationCommand = resizeAllocationCommand;
		}
		
		#region IModifiable

		public INonAllocPool<T> Contents { get => nonAllocPool; }
		
		public void UpdateContents(INonAllocPool<T> newContents)
		{
			nonAllocPool = newContents;
		}
		
		public void UpdateCount(int newCount)
		{
			poolAsModifiable.UpdateCount(newCount);
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

		private Func<T> topUpAllocationDelegate;

		public void TopUp(IPoolElement<T> element)
		{
			element.Value = topUpAllocationDelegate.Invoke();
		}

		#endregion

		#region INonAllocPool

		/*
		public IPoolElement<T> Pop()
		{
			if (!packedArray.HasFreeSpace)
			{
				int previousCapacity = packedArray.Capacity;

				resizeDelegate(this);

				int newCapacity = packedArray.Capacity;
			}

			IPoolElement<T> result = packedArray.Pop();

			return result;
		}
		*/
				
		public IPoolElement<T> Pop()
		{
			if (!nonAllocPool.HasFreeSpace)
			{
				int previousCapacity = poolAsFixedSizeCollection.Capacity;

				resizeDelegate(this);

				int newCapacity = poolAsFixedSizeCollection.Capacity;
			}

			IPoolElement<T> result = nonAllocPool.Pop();

			return result;
		}

		public void Push(IPoolElement<T> instance)
		{
			nonAllocPool.Push(instance);
		}
		
		public bool HasFreeSpace { get { return nonAllocPool.HasFreeSpace; } }

		#endregion
	}
}