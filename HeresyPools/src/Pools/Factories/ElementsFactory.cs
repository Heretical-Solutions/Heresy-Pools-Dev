using HereticalSolutions.Collections.Allocations;

using System;

namespace HereticalSolutions.Pools.Factories
{
	public static partial class PoolsFactory
	{
		#region Pool element allocation command

		public static AllocationCommand<IPoolElement<T>> BuildPoolElementAllocationCommand<T>(
			AllocationCommandDescriptor descriptor,
			Func<T> valueAllocationDelegate,
			Func<Func<T>, IPoolElement<T>> containerAllocationDelegate)
		{
			Func<IPoolElement<T>> poolElementAllocationDelegate = () =>
				containerAllocationDelegate(valueAllocationDelegate);

			var poolElementAllocationCommand = new AllocationCommand<IPoolElement<T>>
			{
				Descriptor = descriptor,

				AllocationDelegate = poolElementAllocationDelegate
			};

			return poolElementAllocationCommand;
		}

		#endregion

		#region Default allocation delegates

		public static T NullAllocationDelegate<T>()
		{
			return default(T);
		}

		#endregion
		
		#region Indexed container

		public static IndexedContainer<T> BuildIndexedContainer<T>(
			Func<T> allocationDelegate)
		{
			IndexedContainer<T> result = new IndexedContainer<T>((allocationDelegate != null)
				? allocationDelegate.Invoke()
				: default(T));

			return result;
		}

		#endregion
	}
}