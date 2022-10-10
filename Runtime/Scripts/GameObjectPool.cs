using System;
using UnityEngine;
using System.Collections.Generic;
using HereticalSolutions.Allocations;
using HereticalSolutions.Collections.Managed;

namespace HereticalSolutions.Pools
{
	public class GameObjectPool : StackPool<GameObject>
	{
		protected GameObject prefab;

		public GameObject Prefab { get => prefab; }

		public GameObjectPool(
            Stack<GameObject> pool,
            GameObject prefab,
			Action<StackPool<GameObject>> resizeDelegate,
			AllocationCommand<GameObject> allocationCommand)
		: base(
            pool,
            resizeDelegate,
			allocationCommand)
		{
			this.prefab = prefab;
		}

		protected override void OnBeforePop(GameObject instance)
		{
			instance.SetActive(true);
		}

		protected override void OnBeforePush(GameObject instance)
		{
			instance.SetActive(false);
		}
	}
}