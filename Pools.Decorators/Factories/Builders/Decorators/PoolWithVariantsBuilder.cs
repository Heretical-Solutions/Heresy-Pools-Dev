using System;

using HereticalSolutions.Pools.Decorators;

using HereticalSolutions.RandomGeneration;
using HereticalSolutions.RandomGeneration.Factories;

using HereticalSolutions.Repositories;

using HereticalSolutions.Repositories.Factories;

namespace HereticalSolutions.Pools.Factories
{
    public class PoolWithVariantsBuilder<T>
    {
	    private IRepository<int, VariantContainer<T>> repository;

	    private IRandomGenerator random;

		public void Initialize()
		{
			repository = RepositoriesFactory.BuildDictionaryRepository<int, VariantContainer<T>>();

			random = RandomFactory.BuildSystemRandomGenerator();
		}

		public void AddVariant(
			int index,
			float chance,
			INonAllocDecoratedPool<T> poolByVariant)
		{
			repository.Add(
				index,
				new VariantContainer<T>
				{
					Chance = chance,

					Pool = poolByVariant
				});
		}


		public INonAllocDecoratedPool<T> Build()
		{
			if (repository == null)
				throw new Exception("[PoolWithVariantsBuilder] BUILDER NOT INITIALIZED");

			var result = PoolsFactory.BuildNonAllocPoolWithVariants<T>(
				repository,
				random);

			repository = null;

			random = null;

			return result;
		}
    }
}