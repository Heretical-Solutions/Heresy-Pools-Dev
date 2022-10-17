using System;
using System.Collections.Generic;
using UnityEngine;

namespace HereticalSolutions.Pools.Factories
{
	public class NonAllocDecoratedPoolBuilder<T>
	{
		public INonAllocDecoratedPool<T> CurrentWrapper { get; private set; }

		public NonAllocDecoratedPoolBuilder<T> Add(INonAllocDecoratedPool<T> newWrapper)
		{
			CurrentWrapper = newWrapper;

			return this;
		}
	}
}