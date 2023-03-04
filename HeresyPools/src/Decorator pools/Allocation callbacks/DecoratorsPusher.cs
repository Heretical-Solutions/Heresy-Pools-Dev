using HereticalSolutions.Collections;

namespace HereticalSolutions.Pools.AllocationCallbacks
{
	public class DecoratorsPusher<T> : IAllocationCallback<T>
	{
		public void OnAllocated(
			INonAllocDecoratedPool<T> rootPoolDecorator,
			IPoolElement<T> currentElement)
		{
			if (currentElement.Value == null)
				return;

			if (((IIndexed)currentElement).Index == -1)
				rootPoolDecorator.Push(
					currentElement,
					true);
		}
	}
}