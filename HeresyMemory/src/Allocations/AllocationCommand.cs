using System;

namespace HereticalSolutions.Collections.Allocations
{
	public class AllocationCommand<T>
	{
		public AllocationCommandDescriptor Descriptor;

		public Func<T> AllocationDelegate;
	}
}