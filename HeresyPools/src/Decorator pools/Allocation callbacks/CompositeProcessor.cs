using System.Collections.Generic;

namespace HereticalSolutions.Pools.AllocationCallbacks
{
	public class CompositeProcessor<T>
		: INotifiable<T>,
		  IPoolProvidable<T>
	{
		protected INonAllocDecoratedPool<T> poolWrapper = null;

		protected Stack<IPoolElement<T>> processingQueue;

		protected IAllocationCallback<T>[] processors;

		public CompositeProcessor(
			Stack<IPoolElement<T>> processingQueue,
			IAllocationCallback<T>[] processors)
		{
			this.processingQueue = processingQueue;

			this.processors = processors;
		}

		public void Notify(IPoolElement<T> element)
		{
			if (poolWrapper != null)
			{
				foreach (var processor in processors)
					processor.OnAllocated(
						poolWrapper,
						element);

				return;
			}

			processingQueue.Push(element);
		}

		public void SetPool(INonAllocDecoratedPool<T> pool)
		{
			poolWrapper = pool;

			if (processingQueue.Count == 0)
				return;

			while (processingQueue.Count != 0)
			{
				var element = processingQueue.Pop();

				foreach (var processor in processors)
					processor.OnAllocated(
						poolWrapper,
						element);
			}
		}
	}
}