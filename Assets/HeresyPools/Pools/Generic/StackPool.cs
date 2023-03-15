using System;
using System.Collections.Generic;

using HereticalSolutions.Collections;
using HereticalSolutions.Collections.Allocations;

namespace HereticalSolutions.Pools.Generic
{
	public class StackPool<T> 
		: IPool<T>,
		  IResizable<T>,
		  IModifiable<Stack<T>>,
		  ICountUpdateable
	{
		private Stack<T> pool;

		public StackPool(
			Stack<T> pool,
			Action<StackPool<T>> resizeDelegate,
			AllocationCommand<T> allocationCommand)
		{
			this.pool = pool;

			this.resizeDelegate = resizeDelegate;

			ResizeAllocationCommand = allocationCommand;
		}
		
		#region IModifiable

		public Stack<T> Contents { get => pool; }

		public void UpdateContents(Stack<T> newContents)
		{
			pool = newContents;
		}

		public void UpdateCount(int newCount)
		{
			throw new Exception("[StackPool] CANNOT UPDATE COUNT OF STACK");
		}

		#endregion
		
		#region IResizable

		public AllocationCommand<T> ResizeAllocationCommand { get; private set; }

		private readonly Action<StackPool<T>> resizeDelegate;

		public void Resize()
		{
			resizeDelegate(this);
		}

		#endregion

		#region IPool

		public T Pop()
		{
			T result = default(T);

			if (pool.Count != 0)
			{
				result = pool.Pop();
			}
			else
			{
				resizeDelegate(this);

				result = pool.Pop();
			}
			
			return result;
		}

		public void Push(T instance)
		{
			pool.Push(instance);
		}
		
		public bool HasFreeSpace { get { return true; } } // ¯\_(ツ)_/¯

		#endregion
	}
}