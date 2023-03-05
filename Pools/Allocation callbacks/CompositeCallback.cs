namespace HereticalSolutions.Pools.AllocationCallbacks
{
	public class CompositeCallback<T> : IAllocationCallback<T>
	{
		private readonly IAllocationCallback<T>[] callbacks;

		public CompositeCallback(IAllocationCallback<T>[] callbacks)
		{
			this.callbacks = callbacks;
		}

		public void OnAllocated(IPoolElement<T> element)
		{
			foreach (var processor in callbacks)
				processor.OnAllocated(
					element);
		}
	}
}