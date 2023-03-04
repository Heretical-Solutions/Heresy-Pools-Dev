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

			if (currentElement.Metadata.Get<IIndexed>().Index == -1)
				rootPoolDecorator.Push(
					currentElement,
					true);
		}
	}
}