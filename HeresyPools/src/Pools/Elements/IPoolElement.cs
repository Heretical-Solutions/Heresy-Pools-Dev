using System;
using HeresyPools.Pools;
using HereticalSolutions.Repositories;

namespace HereticalSolutions.Pools
{
	public interface IPoolElement<T>
	{
		T Value { get; set; }

		EPoolElementStatus Status { get; }

		IMetadata Metadata { get; }

		void Push();
	}
}