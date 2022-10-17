using HereticalSolutions.Collections;
using HereticalSolutions.Pools.Arguments;

namespace HereticalSolutions.Pools
{
	public class NonAllocWrapperPool<T> : ANonAllocDecoratorPool<T>
	{
		private INonAllocPool<T> nonAllocPool;

		public NonAllocWrapperPool(INonAllocPool<T> nonAllocPool)
			: base(null)
		{
			this.nonAllocPool = nonAllocPool;
		}

		public override IPoolElement<T> Pop(params IPoolDecoratorArgument[] args)
		{
			return nonAllocPool.Pop();
		}

		public override void Push(IPoolElement<T> instance)
		{
			nonAllocPool.Push(instance);
		}
	}
}