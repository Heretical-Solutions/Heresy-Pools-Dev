using System;
using HereticalSolutions.Collections;
using HereticalSolutions.Repositories;
using HereticalSolutions.Pools.Arguments;

namespace HereticalSolutions.Pools
{
	public class VariantContainer<T>
	{
		public float Chance;

		public INonAllocDecoratedPool<T> Pool;
	}
}