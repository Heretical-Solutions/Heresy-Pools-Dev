using UnityEngine;

using HereticalSolutions.Collections;
using HereticalSolutions.Collections.Managed;

namespace HereticalSolutions.Pools.AllocationProcessors
{
	public class NameByStringAndIndex : IAllocationProcessor
	{
		private string name;

		private int index = 0;

		public NameByStringAndIndex(string name)
		{
			this.name = name;
		}

		public void Process(
			INonAllocDecoratedPool<GameObject> poolWrapper,
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