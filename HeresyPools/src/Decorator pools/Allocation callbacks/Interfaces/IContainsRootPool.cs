namespace HereticalSolutions.Pools
{
	public interface IContainsRootPool<T>
	{
		void SetRootPool(INonAllocDecoratedPool<T> pool);
	}
}