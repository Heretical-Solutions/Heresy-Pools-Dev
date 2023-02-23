namespace HereticalSolutions.Pools.AllocationCallbacks
{
	public interface IAllocationCallback<T>
	{
		void OnAllocated(
			INonAllocDecoratedPool<T> poolWrapper,
			IPoolElement<T> currentElement);
	}
}