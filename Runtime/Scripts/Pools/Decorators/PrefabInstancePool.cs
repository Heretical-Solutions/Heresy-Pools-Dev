using UnityEngine;
using HereticalSolutions.Collections;

namespace HereticalSolutions.Pools
{
	public class PrefabInstancePool : ADecoratorPool<GameObject>
	{
		protected GameObject prefab;

		public GameObject Prefab { get => prefab; }

		public PrefabInstancePool(
			IPool<GameObject> innerPool,
			GameObject prefab)
			: base(innerPool)
		{
			this.prefab = prefab;
		}
	}
}