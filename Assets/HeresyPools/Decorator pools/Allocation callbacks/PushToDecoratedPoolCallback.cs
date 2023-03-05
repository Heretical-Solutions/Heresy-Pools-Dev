namespace HereticalSolutions.Pools.AllocationCallbacks
{
	public class PushToDecoratedPoolCallback<T> : IAllocationCallback<T>
	{
		public INonAllocDecoratedPool<T> Root { get; set; }

		public void OnAllocated(IPoolElement<T> currentElement)
		{
			if (currentElement.Value == null)
				return;

			Root.Push(
				currentElement,
				true);
		}
	}
}