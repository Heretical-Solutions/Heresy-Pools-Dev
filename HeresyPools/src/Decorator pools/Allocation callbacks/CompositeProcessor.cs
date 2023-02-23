using System.Collections.Generic;

namespace HereticalSolutions.Pools.AllocationCallbacks
{
	public class CompositeProcessor<T>
		: INotifiable<T>,
		  IContainsRootPool<T>
	{
		protected INonAllocDecoratedPool<T> rootPool = null;

		protected Stack<IPoolElement<T>> elementsToProcess;

		protected IAllocationCallback<T>[] callbacks;

		public CompositeProcessor(
			Stack<IPoolElement<T>> elementsToProcess,
			IAllocationCallback<T>[] callbacks)
		{
			this.elementsToProcess = elementsToProcess;

			this.callbacks = callbacks;
		}

		public void Notify(IPoolElement<T> element)
		{
			if (rootPool != null)
			{
				foreach (var processor in callbacks)
					processor.OnAllocated(
						rootPool,
						element);

				return;
			}

			elementsToProcess.Push(element);
		}

		public void SetRootPool(INonAllocDecoratedPool<T> pool)
		{
			rootPool = pool;

			if (elementsToProcess.Count == 0)
				return;

			while (elementsToProcess.Count != 0)
			{
				var element = elementsToProcess.Pop();

				foreach (var processor in callbacks)
					processor.OnAllocated(
						rootPool,
						element);
			}
		}
	}
}