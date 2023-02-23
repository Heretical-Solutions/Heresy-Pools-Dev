using HereticalSolutions.Pools.Arguments;

namespace HereticalSolutions.Pools
{
	public class WrapperPool<T> : ADecoratorPool<T>
	{
		private IPool<T> pool;

		public WrapperPool(IPool<T> pool)
			: base(null)
		{
			this.pool = pool;
		}

		public override T Pop(IPoolDecoratorArgument[] args)
		{
			return pool.Pop();
		}

		public override void Push(
			T instance,
			bool dryRun = false)
		{
			if (!dryRun)
				pool.Push(instance);
		}
	}
}