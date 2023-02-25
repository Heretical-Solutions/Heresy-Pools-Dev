using System;
using HereticalSolutions.Repositories;
using HereticalSolutions.Pools.Arguments;
using HereticalSolutions.Random;

namespace HereticalSolutions.Pools
{
	public class NonAllocPoolWithVariants<T>
		: INonAllocDecoratedPool<T>
	{
		private IRepository<int, VariantContainer<T>> innerPoolsRepository;

		private IRandomGenerator randomGenerator;

		public NonAllocPoolWithVariants(
			IRepository<int, VariantContainer<T>> innerPoolsRepository,
			IRandomGenerator randomGenerator)
		{
			this.innerPoolsRepository = innerPoolsRepository;

			this.randomGenerator = randomGenerator;
		}

		#region Pop

		public IPoolElement<T> Pop(IPoolDecoratorArgument[] args)
		{
			if (args.TryGetArgument<VariantArgument>(out var arg))
			{
				if (!innerPoolsRepository.TryGet(arg.Variant, out var variant))
					throw new Exception($"[PoolWithVariants] INVALID VARIANT {{ {arg.Variant} }}");

				var concreteResult = variant.Pool.Pop(args);

				return concreteResult;
			}

			if (!innerPoolsRepository.TryGet(0, out var currentVariant))
				throw new Exception("[PoolWithVariants] NO VARIANTS PRESENT");

			var hitDice = randomGenerator.Random(0, 1f);

			int index = 0;

			while (currentVariant.Chance < hitDice)
			{
				hitDice -= currentVariant.Chance;

				index++;

				if (!innerPoolsRepository.TryGet(index, out currentVariant))
					throw new Exception("[PoolWithVariants] INVALID VARIANT CHANCES");
			}

			var result = currentVariant.Pool.Pop(args);

			return result;
		}

		#endregion

		#region Push

		public void Push(
			IPoolElement<T> instance,
			bool dryRun = false)
		{
			var elementWithVariant = (IContainsVariant)instance;

			if (elementWithVariant == null)
				throw new Exception("[PoolWithVariants] INVALID INSTANCE");

			if (!innerPoolsRepository.TryGet(elementWithVariant.Variant, out var variant))
				throw new Exception($"[PoolWithVariants] INVALID VARIANT {{ {elementWithVariant.Variant} }}");

			variant.Pool.Push(
				instance,
				dryRun);
		}
		
		#endregion
	}
}