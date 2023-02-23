using System;
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

		public override IPoolElement<T> Pop(IPoolDecoratorArgument[] args)
		{
			if (args.TryGetArgument<AppendArgument>(out var arg))
			{
				var appendable = (IAppendable<IPoolElement<T>>)nonAllocPool;

				if (appendable == null)
					throw new Exception("[NonAllocWrapperPool] POOL IS NOT APPENDABLE");

				var appendee = appendable.Append();

				return appendee;
			}

			var result = nonAllocPool.Pop();

			if (result.Value.Equals(default(IPoolElement<T>)))
			{
				var topUppable = (ITopUppable<IPoolElement<T>>)nonAllocPool;

				if (topUppable == null)
					throw new Exception("[NonAllocWrapperPool] POOL ELEMENT IS EMPTY");
				
				topUppable.TopUp(result);
			}

			return result;
		}

		public override void Push(
			IPoolElement<T> instance,
			bool dryRun = false)
		{
			if (!dryRun)
				nonAllocPool.Push(instance);
		}
	}
}