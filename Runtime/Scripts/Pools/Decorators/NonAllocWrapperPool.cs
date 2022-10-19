using System;
using HereticalSolutions.Collections;
using HereticalSolutions.Pools.Arguments;
using HereticalSolutions.Allocations.Internal;

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

		public override IPoolElement<T> Pop(IPoolDecoratorArgument[] args)
		{
			if (args.TryGetArgument<AppendArgument>(out var arg))
			{
				var appendable = (IAppendable<IPoolElement<T>>)nonAllocPool;

				if (appendable == null)
					throw new Exception("[NonAllocWrapperPool] Pool is not appendable");

				var appendee = appendable.Append();

				return appendee;
			}

			var result = nonAllocPool.Pop();

			if (result.Value.Equals(default(T)))
			{
				var topUppable = (ITopUppable<T>)nonAllocPool;

				if (topUppable == null)
					throw new Exception("[NonAllocWrapperPool] Pool element is empty");
				
				topUppable.TopUp(result);
			}

			return result;
		}

		public override void Push(IPoolElement<T> instance)
		{
			nonAllocPool.Push(instance);
		}
	}
}