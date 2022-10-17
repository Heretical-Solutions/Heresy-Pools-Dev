using UnityEngine;

namespace HereticalSolutions.Pools
{
	public class PrefabInstancePool : ADecoratorPool<GameObject>
	{
		protected GameObject prefab;

		public GameObject Prefab { get => prefab; }

		public PrefabInstancePool(
			IDecoratedPool<GameObject> innerPool,
			GameObject prefab)
			: base(innerPool)
		{
			this.prefab = prefab;
		}
	}
}