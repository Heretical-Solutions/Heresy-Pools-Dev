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
		private static INonAllocPool<GameObject> BuildGameObjectNonAllocPool(
			GameObject prefab,
			AllocationCommandDescriptor initialAllocation,
			AllocationCommandDescriptor additionalAllocation,
			Func<Func<GameObject>, IPoolElement<GameObject>> containerAllocationDelegate)
		{
			Func<GameObject> valueAllocationDelegate = () => { return GameObject.Instantiate(prefab); };

			return BuildNonAllocPool<GameObject>(
				valueAllocationDelegate,
				containerAllocationDelegate,
				initialAllocation,
				additionalAllocation);
		}

		private static INonAllocPool<GameObject> BuildGameObjectNonAllocPool(
			GameObject prefab,
			DiContainer container,
			AllocationCommandDescriptor initialAllocation,
			AllocationCommandDescriptor additionalAllocation,
			Func<Func<GameObject>, IPoolElement<GameObject>> containerAllocationDelegate)
		{
			Func<GameObject> valueAllocationDelegate = () => { return container.InstantiatePrefab(prefab); };

			return BuildNonAllocPool<GameObject>(
				valueAllocationDelegate,
				containerAllocationDelegate,
				initialAllocation,
				additionalAllocation);
		}

		public static INonAllocPool<T> BuildNonAllocPool<T>(
			Func<T> valueAllocationDelegate,
			Func<Func<T>, IPoolElement<T>> containerAllocationDelegate,
			AllocationCommandDescriptor initialAllocation,
			AllocationCommandDescriptor additionalAllocation)
		{
			INonAllocPool<T> packedArrayPool = CollectionFactory.BuildIndexedPackedArrayPool<T>(

				CollectionFactory.BuildPoolElementAllocationCommand<T>(
					initialAllocation,
					valueAllocationDelegate,
					containerAllocationDelegate),

				CollectionFactory.BuildPoolElementAllocationCommand<T>(
					additionalAllocation,
					valueAllocationDelegate,
					containerAllocationDelegate));

			return packedArrayPool;
		}

		public static PoolElementWithVariant<T> BuildPoolElementWithVariant<T>(
			Func<T> allocationDelegate)
		{
			PoolElementWithVariant<T> result = new PoolElementWithVariant<T>(allocationDelegate());

			return result;
		}
	}
}