using UnityEngine;
using System.Collections.Generic;
using HereticalSolutions.Collections;
using HereticalSolutions.Allocations;

namespace HereticalSolutions.Pools.Notifiables
{
	public interface IAllocationProcessor
	{
		void Process(
			INonAllocDecoratedPool<GameObject> poolWrapper,
			IPoolElement<GameObject> currentElement,
			IPoolElement<GameObject> poppedElement = null);
	}
}