using HereticalSolutions.Repositories;

namespace HereticalSolutions.Pools
{
	public interface IPoolElement<T>
	{
		T Value { get; set; }

		EPoolElementStatus Status { get; }

		IReadOnlyObjectRepository Metadata { get; }

		void Push();
	}
}