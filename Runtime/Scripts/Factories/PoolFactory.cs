using System;
using UnityEngine;
using Zenject;

using HereticalSolutions.Collections;
using HereticalSolutions.Collections.Factories;

using HereticalSolutions.Allocations;

namespace HereticalSolutions.Pools.Factories
{
	public static class PoolFactory
	{
		public static INonAllocPool<GameObject> BuildGameObjectNonAllocPool(
			GameObject prefab,
			AllocationCommandDescriptor initialAllocation,
			AllocationCommandDescriptor additionalAllocation,
			Func<Func<GameObject>, IPoolElement<GameObject>> containerAllocationDelegate)
		{
			Func<GameObject> valueAllocationDelegate = () => { return GameObject.Instantiate(prefab); };

			return CollectionFactory.BuildNonAllocPool<GameObject>(
				valueAllocationDelegate,
				containerAllocationDelegate,
				initialAllocation,
				additionalAllocation);
		}

		public static INonAllocPool<GameObject> BuildGameObjectNonAllocPool(
			GameObject prefab,
			DiContainer container,
			AllocationCommandDescriptor initialAllocation,
			AllocationCommandDescriptor additionalAllocation,
			Func<Func<GameObject>, IPoolElement<GameObject>> containerAllocationDelegate)
		{
			Func<GameObject> valueAllocationDelegate = () => { return container.InstantiatePrefab(prefab); };

			return CollectionFactory.BuildNonAllocPool<GameObject>(
				valueAllocationDelegate,
				containerAllocationDelegate,
				initialAllocation,
				additionalAllocation);
		}

		public static PoolElementWithVariant<T> BuildPoolElementWithVariant<T>(
			Func<T> allocationDelegate)
		{
			PoolElementWithVariant<T> result = new PoolElementWithVariant<T>(allocationDelegate());

			return result;
		}
	}
}