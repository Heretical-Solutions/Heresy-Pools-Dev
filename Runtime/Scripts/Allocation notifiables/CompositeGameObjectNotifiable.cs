using UnityEngine;
using System.Collections.Generic;
using HereticalSolutions.Collections;
using HereticalSolutions.Allocations;

namespace HereticalSolutions.Pools.Notifiables
{
	public class CompositeGameObjectNotifiable : IAllocationNotifiable<GameObject>
	{
		protected INonAllocDecoratedPool<GameObject> poolWrapper = null;

		protected Stack<IPoolElement<GameObject>> allocatedElements;

		protected IAllocationProcessor[] processors;

		protected bool dirty;

		protected bool handlingInProgress = false;

		public void SetWrapper(INonAllocDecoratedPool<GameObject> poolWrapper)
		{
			this.poolWrapper = poolWrapper;
		}

		public CompositeGameObjectNotifiable(
			Stack<IPoolElement<GameObject>> allocatedElements,
			IAllocationProcessor[] processors)
		{
			this.allocatedElements = allocatedElements;

			this.processors = processors;
		}

		public void Notify(IPoolElement<GameObject> element)
		{
			allocatedElements.Push(element);

			dirty = true;
		}

		public void Process(IPoolElement<GameObject> poppedElement = null)
		{
			if (poolWrapper == null)
				return;

			if (!dirty)
				return;

			if (handlingInProgress)
				return;

			handlingInProgress = true;

			for (int i = 0; i < allocatedElements.Count; i++)
			{
				var element = allocatedElements.Pop();

				foreach (var processor in processors)
					processor.Process(
						poolWrapper,
						element,
						poppedElement);
			}

			handlingInProgress = false;
		}
	}
}