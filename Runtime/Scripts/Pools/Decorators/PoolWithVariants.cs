using System;
using HereticalSolutions.Collections;
using HereticalSolutions.Repositories;
using HereticalSolutions.Pools.Arguments;

namespace HereticalSolutions.Pools
{
	public class PoolWithVariants<T>
		: INonAllocDecoratedPool<T>
	{
		private IRepository<int, INonAllocDecoratedPool<T>> poolsRepository;

		public PoolWithVariants(
			IRepository<int, INonAllocDecoratedPool<T>> poolsRepository)
		{
			this.poolsRepository = poolsRepository;
		}

		#region Pop

		public IPoolElement<T> Pop(IPoolDecoratorArgument[] args)
		{
			if (!args.TryGetArgument<VariantArgument>(out var arg))
				throw new Exception("[PoolWithVariants] VARIANT ARGUMENT ABSENT");

			if (!poolsRepository.TryGet(arg.Variant, out var pool))
				throw new Exception($"[PoolWithVariants] INVALID VARIANT {{ {arg.Variant} }}");

			var result = pool.Pop(args);

			((PoolElementWithVariant<T>)result).Variant = arg.Variant;

			return result;
		}

		#endregion

		#region Push

		public void Push(IPoolElement<T> instance)
		{
			var elementWithVariant = (PoolElementWithVariant<T>)instance;

			if (elementWithVariant == null)
				throw new Exception("[PoolWithVariants] INVALID INSTANCE");

			if (!poolsRepository.TryGet(elementWithVariant.Variant, out var pool))
				throw new Exception($"[PoolWithVariants] INVALID VARIANT {{ {elementWithVariant.Variant} }}");

			pool.Push(instance);
		}
		
		#endregion
	}
}