using HereticalSolutions.Collections.Allocations;

namespace HereticalSolutions.Collections
{
	public interface IResizable<T>
	{
		AllocationCommand<T> ResizeAllocationCommand { get; }

		void Resize();
	}
}