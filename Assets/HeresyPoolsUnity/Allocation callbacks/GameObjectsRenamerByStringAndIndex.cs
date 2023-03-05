using UnityEngine;

using HereticalSolutions.Collections;
using HereticalSolutions.Collections.Managed;

namespace HereticalSolutions.Pools.AllocationCallbacks
{
	public class GameObjectsRenamerByStringAndIndex : IAllocationCallback<GameObject>
	{
		private string name;

		private int index = 0;

		public GameObjectsRenamerByStringAndIndex(string name)
		{
			this.name = name;
		}

		public void OnAllocated(
			INonAllocDecoratedPool<GameObject> rootPoolDecorator,
			IPoolElement<GameObject> currentElement)
		{
			if (currentElement.Value == null)
				return;

			if (((IIndexed)currentElement).Index == -1)
			{
				currentElement.Value.name = $"{name} {index.ToString()}";

				index++;
			}
		}
	}
}