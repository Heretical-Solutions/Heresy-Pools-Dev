using System;

namespace HereticalSolutions.Collections.Allocations
{
	[Serializable]
	public struct AllocationCommandDescriptor
	{
		public EAllocationAmountRule Rule;
		
		public int Amount;
	}
}