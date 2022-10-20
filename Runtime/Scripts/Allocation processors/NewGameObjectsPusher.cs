using UnityEngine;
using System.Collections.Generic;

using HereticalSolutions.Collections;
using HereticalSolutions.Collections.Managed;

using HereticalSolutions.Allocations;

namespace HereticalSolutions.Pools.AllocationProcessors
{
	public class NewGameObjectsPusher : IAllocationProcessor
	{
		public void Process(
			INonAllocDecoratedPool<GameObject> poolWrapper,
			IPoolElement<GameObject> currentElement)
		{
			if (((IIndexed)currentElement).Index == -1)
				poolWrapper.Push(
					currentElement,
					true);
		}
	}
}