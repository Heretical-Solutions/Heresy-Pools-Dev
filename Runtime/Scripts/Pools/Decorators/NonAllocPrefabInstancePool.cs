using UnityEngine;
using HereticalSolutions.Collections;

namespace HereticalSolutions.Pools
{
	public class NonAllocPrefabInstancePool : ANonAllocDecoratorPool<GameObject>
	{
		protected GameObject prefab;

		public GameObject Prefab { get => prefab; }

		public NonAllocPrefabInstancePool(
			INonAllocPool<GameObject> innerPool,
			GameObject prefab)
			: base(innerPool)
		{
			this.prefab = prefab;
		}
	}
}