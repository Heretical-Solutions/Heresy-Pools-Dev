using HereticalSolutions.Collections;
using HereticalSolutions.Collections.Managed;

namespace HereticalSolutions.Pools
{
	public class DryRunner<T>
	{
		public INonAllocDecoratedPool<T> Pool { get; set; }

		public void OnAllocation(IPoolElement<T> element)
		{
			Pool.Push(element, true);
		}
	}
}