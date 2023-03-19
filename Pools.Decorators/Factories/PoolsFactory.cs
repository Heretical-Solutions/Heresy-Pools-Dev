using HereticalSolutions.Pools.Decorators;
using HereticalSolutions.RandomGeneration;
using HereticalSolutions.Repositories;

namespace HereticalSolutions.Pools.Factories
{
	public static partial class PoolsFactory
	{
		#region Decorator pools

		public static DecoratorPool<T> BuildDecoratorPool<T>(IPool<T> innerPool)
		{
			return new DecoratorPool<T>(innerPool);
		}

		public static PoolWithID<T> BuildPoolWithID<T>(
			IDecoratedPool<T> innerPool,
			string id)
		{
			return new PoolWithID<T>(innerPool, id);
		}

		#endregion

		#region Non alloc decorator pools

		public static NonAllocPoolWithID<T> BuildNonAllocPoolWithID<T>(
			INonAllocDecoratedPool<T> innerPool,
			string id)
		{
			return new NonAllocPoolWithID<T>(innerPool, id);
		}
		
		public static NonAllocPoolWithAddress<T> BuildNonAllocPoolWithIdAddress<T>(
			IRepository<int, INonAllocDecoratedPool<T>> repository,
			int level)
		{
			return new NonAllocPoolWithAddress<T>(repository, level);
		}
		
		public static NonAllocPoolWithVariants<T> BuildNonAllocPoolWithVariants<T>(
			IRepository<int, VariantContainer<T>> repository,
			IRandomGenerator generator)
		{
			return new NonAllocPoolWithVariants<T>(repository, generator);
		}

		#endregion
	}
}