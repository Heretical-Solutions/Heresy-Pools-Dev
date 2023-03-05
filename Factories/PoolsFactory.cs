using UnityEngine;

using HereticalSolutions.Pools.Decorators;

namespace HereticalSolutions.Pools.Factories
{
	public static partial class PoolsFactory
	{
		#region Decorator pools

		public static GameObjectPool BuildGameObjectPool(
			IDecoratedPool<GameObject> innerPool,
			Transform parentTransform = null)
		{
			return new GameObjectPool(
				innerPool,
				parentTransform);
		}

		public static PrefabInstancePool BuildPrefabInstancePool(
			IDecoratedPool<GameObject> innerPool,
			GameObject prefab)
		{
			return new PrefabInstancePool(
				innerPool,
				prefab);
		}

		#endregion

		#region Non alloc decorator pools

		public static NonAllocGameObjectPool BuildNonAllocGameObjectPool(
			INonAllocDecoratedPool<GameObject> innerPool,
			Transform parentTransform = null)
		{
			return new NonAllocGameObjectPool(
				innerPool,
				parentTransform);
		}

		public static NonAllocPrefabInstancePool BuildNonAllocPrefabInstancePool(
			INonAllocDecoratedPool<GameObject> innerPool,
			GameObject prefab)
		{
			return new NonAllocPrefabInstancePool(
				innerPool,
				prefab);
		}

		#endregion
	}
}