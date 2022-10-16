using UnityEngine;
using HereticalSolutions.Collections;

namespace HereticalSolutions.Pools
{
	public class NonAllocGameObjectPool : ANonAllocDecoratorPool<GameObject>
	{
		public NonAllocGameObjectPool(INonAllocPool<GameObject> innerPool)
			: base(innerPool)
		{
		}

		protected override void OnAfterPop(IPoolElement<GameObject> instance)
		{
			instance.Value.SetActive(true);
		}

		protected override void OnBeforePush(IPoolElement<GameObject> instance)
		{
			instance.Value.SetActive(false);
		}
	}
}