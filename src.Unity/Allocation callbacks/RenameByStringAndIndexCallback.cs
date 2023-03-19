using UnityEngine;

using HereticalSolutions.Collections;

namespace HereticalSolutions.Pools.AllocationCallbacks
{
	public class RenameByStringAndIndexCallback : IAllocationCallback<GameObject>
	{
		private readonly string name;

		private int index = 0;

		public RenameByStringAndIndexCallback(string name)
		{
			this.name = name;
		}

		public void OnAllocated(
			IPoolElement<GameObject> currentElement)
		{
			if (currentElement.Value == null)
				return;

			if (currentElement.Metadata.Get<IIndexed>().Index == -1)
			{
				currentElement.Value.name = $"{name} {index.ToString()}";

				index++;
			}
		}
	}
}