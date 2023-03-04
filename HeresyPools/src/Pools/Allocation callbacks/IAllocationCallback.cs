namespace HereticalSolutions.Pools
{
	public interface IAllocationCallback<T>
	{
		void OnAllocated(IPoolElement<T> poolElement);
	}
}