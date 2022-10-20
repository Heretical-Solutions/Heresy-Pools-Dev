using System.Collections.Generic;
using HereticalSolutions.Collections;
using HereticalSolutions.Collections.Managed;
using HereticalSolutions.Allocations;

namespace HereticalSolutions.Pools
{
	public class DryRunner<T> : IAllocationNotifiable<T>
	{
		private INonAllocDecoratedPool<T> poolWrapper = null;

		private Stack<IPoolElement<T>> elementsWithAllocations;

		public bool Dirty { get { return elementsWithAllocations.Count > 0; } }

		public DryRunner(Stack<IPoolElement<T>> elementsWithAllocations)
		{
			this.elementsWithAllocations = elementsWithAllocations;
		}

		public void Notify(IPoolElement<T> element)
		{
			elementsWithAllocations.Push(element);
		}

		public void DryRun(IPoolElement<T> elementToExclude)
		{
			for (int i = 0; i < elementsWithAllocations.Count; i++)
			{
				var element = elementsWithAllocations.Pop();

				if (elementToExclude == null
					|| (element != elementToExclude))
					poolWrapper?.Push(
						element,
						true);
			}
		}
	}
}