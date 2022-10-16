using UnityEngine;
using HereticalSolutions.Collections;

namespace HereticalSolutions.Pools
{
	public class GameObjectPool : ADecoratorPool<GameObject>
	{
		public GameObjectPool(IPool<GameObject> innerPool)
			: base(innerPool)
		{
		}

		protected override void OnAfterPop(GameObject instance)
		{
			instance.SetActive(true);
		}

		protected override void OnBeforePush(GameObject instance)
		{
			instance.SetActive(false);
		}
	}
}