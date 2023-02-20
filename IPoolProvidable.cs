namespace HereticalSolutions.Pools
{
	public interface IPoolProvidable<T>
	{
		void SetPool(INonAllocDecoratedPool<T> pool);
	}
}