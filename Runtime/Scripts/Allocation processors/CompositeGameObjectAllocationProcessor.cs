using UnityEngine;
using System.Collections.Generic;
using HereticalSolutions.Collections;
using HereticalSolutions.Allocations;
using HereticalSolutions.Pools;

namespace HereticalSolutions.Pools.AllocationProcessors
{
	public class CompositeGameObjectAllocationProcessor
		: IValueAssignedNotifiable<GameObject>
	{
		protected INonAllocDecoratedPool<GameObject> poolWrapper = null;

		protected Stack<IPoolElement<GameObject>> processingQueue;

		protected IAllocationProcessor[] processors;

		public CompositeGameObjectAllocationProcessor(
			Stack<IPoolElement<GameObject>> processingQueue,
			IAllocationProcessor[] processors)
		{
			this.processingQueue = processingQueue;

			this.processors = processors;
		}

		public void Notify(IPoolElement<GameObject> element)
		{
			if (poolWrapper != null)
			{
				foreach (var processor in processors)
					processor.Process(
						poolWrapper,
						element);

				return;
			}

			processingQueue.Push(element);
		}

		public void SetWrapper(INonAllocDecoratedPool<GameObject> poolWrapper)
		{
			this.poolWrapper = poolWrapper;

			if (processingQueue.Count == 0)
				return;

			while (processingQueue.Count != 0)
			{
				var element = processingQueue.Pop();

				foreach (var processor in processors)
					processor.Process(
						poolWrapper,
						element);
			}
		}
	}
}