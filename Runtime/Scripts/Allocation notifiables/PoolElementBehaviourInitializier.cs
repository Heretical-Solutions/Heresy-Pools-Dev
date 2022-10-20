using UnityEngine;
using System.Collections.Generic;
using HereticalSolutions.Collections;
using HereticalSolutions.Allocations;

namespace HereticalSolutions.Pools.Notifiables
{
	public class PoolElementBehaviourInitializier : IAllocationProcessor
	{
		public void Process(
			INonAllocDecoratedPool<GameObject> poolWrapper,
			IPoolElement<GameObject> currentElement,
			IPoolElement<GameObject> poppedElement = null)
		{
			var behaviour = currentElement.Value.GetComponent<PoolElementBehaviour>();

			if (!behaviour.Initialized)
				behaviour.Initialize(poolWrapper, currentElement);
		}
	}
}