using System;
using HereticalSolutions.Collections;
using HereticalSolutions.Pools.Arguments;

namespace HereticalSolutions.Pools.Decorators
{
	public class NonAllocDecoratorPool<T> : INonAllocDecoratedPool<T>
	{
		private readonly INonAllocPool<T> innerPool;

		public NonAllocDecoratorPool(INonAllocPool<T> innerPool)
		{
			this.innerPool = innerPool;
		}

		public IPoolElement<T> Pop(IPoolDecoratorArgument[] args)
		{
			#region Append from argument
			
			if (args.TryGetArgument<AppendArgument>(out var arg))
			{
				var appendable = (IAppendable<IPoolElement<T>>)innerPool;

				if (appendable == null)
					throw new Exception("[NonAllocDecoratorPool] POOL IS NOT APPENDABLE");

				var appendee = appendable.Append();

				return appendee;
			}
			
			#endregion

			var result = innerPool.Pop();

			#region Top Up from argument

			if (result.Value.Equals(default(T)))
			{
				var topUppable = (ITopUppable<IPoolElement<T>>)innerPool;

				if (topUppable == null)
					throw new Exception("[NonAllocDecoratorPool] POOL ELEMENT IS EMPTY AND POOL IS NOT TOP UPPABLE");
				
				topUppable.TopUp(result);
			}
			
			#endregion

			return result;
		}

		public void Push(
			IPoolElement<T> instance,
			bool decoratorsOnly = false)
		{
			if (!decoratorsOnly)
				innerPool.Push(instance);
		}
	}
}