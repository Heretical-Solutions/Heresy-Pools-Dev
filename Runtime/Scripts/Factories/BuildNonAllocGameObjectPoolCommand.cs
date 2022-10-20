using System;
using UnityEngine;
using HereticalSolutions.Collections;
using HereticalSolutions.Allocations;
using Zenject;

namespace HereticalSolutions.Pools.Factories
{
	public class BuildNonAllocGameObjectPoolCommand
	{
		public GameObject Prefab;
		public DiContainer Container;
		public Type CollectionType;
		public AllocationCommandDescriptor InitialAllocation;
		public AllocationCommandDescriptor AdditionalAllocation;
		public Func<Func<GameObject>, IPoolElement<GameObject>> ContainerAllocationDelegate;
	}
}