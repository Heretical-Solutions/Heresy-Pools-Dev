using System;
using HereticalSolutions.Collections;
using HereticalSolutions.Repositories;
using HereticalSolutions.Pools.Arguments;

namespace HereticalSolutions.Pools
{
	public class PoolWithVariants<T>
		: INonAllocDecoratedPool<T>
	{
		private IRepository<int, VariantContainer<T>> poolsRepository;

		public PoolWithVariants(
			IRepository<int, VariantContainer<T>> poolsRepository)
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

			if (!poolsRepository.TryGet(0, out var currentVariant))
				throw new Exception("[PoolWithVariants] NO VARIANTS PRESENT");

			var hitDice = UnityEngine.Random.Range(0, 1f);

			int index = 0;

			while (currentVariant.Chance < hitDice)
			{
				hitDice -= currentVariant.Chance;

				index++;

				if (!poolsRepository.TryGet(index, out currentVariant))
					throw new Exception("[PoolWithVariants] INVALID VARIANT CHANCES");
			}

			var result = currentVariant.Pool.Pop(args);

			//((PoolElementWithVariant<T>)result).Variant = arg.Variant;

			return result;
		}

		#endregion

		#region Push

		public void Push(
			IPoolElement<T> instance,
			bool dryRun = false)
		{
			var elementWithVariant = (IVariantContainable)instance;

			if (elementWithVariant == null)
				throw new Exception("[PoolWithVariants] INVALID INSTANCE");

			if (!poolsRepository.TryGet(elementWithVariant.Variant, out var variant))
				throw new Exception($"[PoolWithVariants] INVALID VARIANT {{ {elementWithVariant.Variant} }}");

			variant.Pool.Push(
				instance,
				dryRun);
		}
		
		#endregion
	}
}