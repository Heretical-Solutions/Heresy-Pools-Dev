using UnityEngine;
using System.Collections.Generic;
using HereticalSolutions.Collections;
using HereticalSolutions.Allocations;

namespace HereticalSolutions.Pools
{
	public class NewGameObjectsPusher : IAllocationNotifiable<GameObject>
	{
		private INonAllocDecoratedPool<GameObject> poolWrapper = null;

		private Stack<IPoolElement<GameObject>> elementsWithAllocations;

		private bool dirty;

		private bool dryRunInProgress = false;

		public void SetWrapper(INonAllocDecoratedPool<GameObject> poolWrapper)
		{
			this.poolWrapper = poolWrapper;
		}

		public NewGameObjectsPusher(Stack<IPoolElement<GameObject>> elementsWithAllocations)
		{
			this.elementsWithAllocations = elementsWithAllocations;
		}

		public void Notify(IPoolElement<GameObject> element)
		{
			elementsWithAllocations.Push(element);

			dirty = true;
		}

		public void PerformDryRun(IPoolElement<GameObject> elementToExclude = null)
		{
			if (poolWrapper == null)
				return;

			if (!dirty)
				return;

			if (dryRunInProgress)
				return;

			dryRunInProgress = true;

			for (int i = 0; i < elementsWithAllocations.Count; i++)
			{
				var element = elementsWithAllocations.Pop();

				if (elementToExclude == null
					|| (element != elementToExclude))
					poolWrapper.Push(
						element,
						true);
			}

			dryRunInProgress = false;
		}
	}
}