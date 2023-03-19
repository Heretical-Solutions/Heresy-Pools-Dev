using UnityEngine;

namespace HereticalSolutions.Pools.Decorators
{
	public class NonAllocPrefabInstancePool : ANonAllocDecoratorPool<GameObject>
	{
		private readonly GameObject prefab;

		public GameObject Prefab { get => prefab; }

		public NonAllocPrefabInstancePool(
			INonAllocDecoratedPool<GameObject> innerPool,
			GameObject prefab)
			: base(innerPool)
		{
			this.prefab = prefab;
		}
	}
}