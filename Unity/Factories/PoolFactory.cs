using System;
using UnityEngine;

using HereticalSolutions.Collections;
using HereticalSolutions.Collections.Managed;
using HereticalSolutions.Collections.Factories;

using HereticalSolutions.Timers;

namespace HereticalSolutions.Pools.Factories
{
	public static partial class PoolFactory
	{
		#region Non alloc pool

		public static INonAllocPool<GameObject> BuildGameObjectPool(
			BuildNonAllocGameObjectPoolCommand command)
		{
			Func<GameObject> valueAllocationDelegate = (command.Container != null)
				? (Func<GameObject>)(() => { return command.Container.InstantiatePrefab(command.Prefab); })
				: (Func<GameObject>)(() => { return GameObject.Instantiate(command.Prefab); });

			if (command.CollectionType == typeof(PackedArrayPool<GameObject>))
				return CollectionFactory.BuildPackedArrayPool<GameObject>(
					valueAllocationDelegate,
					command.ContainerAllocationDelegate,
					command.InitialAllocation,
					command.AdditionalAllocation);

			if (command.CollectionType == typeof(SupplyAndMergePool<GameObject>))
				return CollectionFactory.BuildSupplyAndMergePool<GameObject>(
					valueAllocationDelegate,
					command.ContainerAllocationDelegate,
					command.InitialAllocation,
					command.AdditionalAllocation);

			throw new Exception($"[PoolFactory] INVALID COLLECTION TYPE: {{ {command.CollectionType.ToString()} }}");
		}

		#endregion
	}
}