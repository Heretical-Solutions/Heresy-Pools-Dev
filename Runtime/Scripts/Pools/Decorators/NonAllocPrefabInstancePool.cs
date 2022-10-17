using UnityEngine;

namespace HereticalSolutions.Pools
{
	public class NonAllocPrefabInstancePool : ANonAllocDecoratorPool<GameObject>
	{
		protected GameObject prefab;

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