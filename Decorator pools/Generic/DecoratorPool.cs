using HereticalSolutions.Pools.Arguments;

namespace HereticalSolutions.Pools
{
	public class DecoratorPool<T> : ADecoratorPool<T>
	{
		private IPool<T> pool;

		public DecoratorPool(IPool<T> pool)
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
			bool decoratorsOnly = false)
		{
			if (!decoratorsOnly)
				pool.Push(instance);
		}
	}
}