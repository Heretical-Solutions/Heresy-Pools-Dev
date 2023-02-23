namespace HereticalSolutions.Pools.AllocationCallbacks
{
	public interface IAllocationCallback<T>
	{
		void OnAllocated(
			INonAllocDecoratedPool<T> rootPoolDecorator,
			IPoolElement<T> currentElement);
	}
}