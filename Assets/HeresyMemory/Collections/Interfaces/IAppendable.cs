using HereticalSolutions.Collections.Allocations;

namespace HereticalSolutions.Collections
{
	public interface IAppendable<T>
	{
		AllocationCommand<T> AppendAllocationCommand { get; }

		T Append();
	}
}