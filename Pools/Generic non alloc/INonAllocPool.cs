namespace HereticalSolutions.Pools
{
	public interface INonAllocPool<T>
	{
		IPoolElement<T> Pop();

		void Push(IPoolElement<T> instance);

		bool HasFreeSpace { get; }
	}
}