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
		#region Packed array pool

		public static INonAllocPool<GameObject> BuildGameObjectPackedArrayPool(
			GameObject prefab,
			AllocationCommandDescriptor initialAllocation,
			AllocationCommandDescriptor additionalAllocation,
			Func<Func<GameObject>, IPoolElement<GameObject>> containerAllocationDelegate)
		{
			Func<GameObject> valueAllocationDelegate = () => { return GameObject.Instantiate(prefab); };

			return CollectionFactory.BuildPackedArrayPool<GameObject>(
				valueAllocationDelegate,
				containerAllocationDelegate,
				initialAllocation,
				additionalAllocation);
		}

		public static INonAllocPool<GameObject> BuildGameObjectPackedArrayPool(
			GameObject prefab,
			DiContainer container,
			AllocationCommandDescriptor initialAllocation,
			AllocationCommandDescriptor additionalAllocation,
			Func<Func<GameObject>, IPoolElement<GameObject>> containerAllocationDelegate)
		{
			Func<GameObject> valueAllocationDelegate = () => { return container.InstantiatePrefab(prefab); };

			return CollectionFactory.BuildPackedArrayPool<GameObject>(
				valueAllocationDelegate,
				containerAllocationDelegate,
				initialAllocation,
				additionalAllocation);
		}

		#endregion

		#region Supply and merge pool

		public static INonAllocPool<GameObject> BuildGameObjectSupplyAndMergePool(
					GameObject prefab,
					AllocationCommandDescriptor initialAllocation,
					AllocationCommandDescriptor additionalAllocation,
					Func<Func<GameObject>, IPoolElement<GameObject>> containerAllocationDelegate)
		{
			Func<GameObject> valueAllocationDelegate = () => { return GameObject.Instantiate(prefab); };

			return CollectionFactory.BuildSupplyAndMergePool<GameObject>(
				valueAllocationDelegate,
				containerAllocationDelegate,
				initialAllocation,
				additionalAllocation);
		}

		public static INonAllocPool<GameObject> BuildGameObjectSupplyAndMergePool(
			GameObject prefab,
			DiContainer container,
			AllocationCommandDescriptor initialAllocation,
			AllocationCommandDescriptor additionalAllocation,
			Func<Func<GameObject>, IPoolElement<GameObject>> containerAllocationDelegate)
		{
			Func<GameObject> valueAllocationDelegate = () => { return container.InstantiatePrefab(prefab); };

			return CollectionFactory.BuildSupplyAndMergePool<GameObject>(
				valueAllocationDelegate,
				containerAllocationDelegate,
				initialAllocation,
				additionalAllocation);
		}

		#endregion

		#region Pool elements

		public static PoolElementWithVariant<T> BuildPoolElementWithVariant<T>(
			Func<T> allocationDelegate)
		{
			PoolElementWithVariant<T> result = new PoolElementWithVariant<T>(allocationDelegate());

			return result;
		}

		#endregion
	}
}