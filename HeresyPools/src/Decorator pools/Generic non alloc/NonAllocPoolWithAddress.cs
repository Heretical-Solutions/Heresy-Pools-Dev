using System;
using HereticalSolutions.Collections;
using HereticalSolutions.Repositories;
using HereticalSolutions.Pools.Arguments;

namespace HereticalSolutions.Pools
{
	public class NonAllocPoolWithAddress<T>
		: INonAllocDecoratedPool<T>
	{
		private int level;

		private IRepository<int, INonAllocDecoratedPool<T>> innerPoolsRepository;

		public NonAllocPoolWithAddress(
			IRepository<int, INonAllocDecoratedPool<T>> innerPoolsRepository,
			int level)
		{
			this.innerPoolsRepository = innerPoolsRepository;

			this.level = level;
		}

		#region Pop

		public IPoolElement<T> Pop(IPoolDecoratorArgument[] args)
		{
			if (!args.TryGetArgument<AddressArgument>(out var arg))
				throw new Exception("[NonAllocPoolWithAddress] ADDRESS ARGUMENT ABSENT");

			if (arg.AddressHashes.Length < level)
				throw new Exception($"[NonAllocPoolWithAddress] INVALID ADDRESS DEPTH. LEVEL: {{ {level} }} ADDRESS LENGTH: {{ {arg.AddressHashes.Length} }}");

			INonAllocDecoratedPool<T> pool = null;

			if (arg.AddressHashes.Length == level)
			{
				if (!innerPoolsRepository.TryGet(0, out pool))
					throw new Exception($"[NonAllocPoolWithAddress] NO POOL DETECTED AT ADDRESS MAX. DEPTH. LEVEL: {{ {level} }}");

				var maxDepthResult = pool.Pop(args);

				return maxDepthResult;
			}

			int currentAddressHash = arg.AddressHashes[level];

			if (!innerPoolsRepository.TryGet(currentAddressHash, out pool))
				throw new Exception($"[NonAllocPoolWithAddress] INVALID ADDRESS {{ {currentAddressHash} }}");

			var result = pool.Pop(args);

			return result;
		}

		#endregion

		#region Push

		public void Push(
			IPoolElement<T> instance,
			bool dryRun = false)
		{
			var elementWithAddress = (IContainsAddress)instance;

			if (elementWithAddress == null)
				throw new Exception("[NonAllocPoolWithAddress] INVALID INSTANCE");

			INonAllocDecoratedPool<T> pool = null;

			if (elementWithAddress.AddressHashes.Length == level)
			{
				if (!innerPoolsRepository.TryGet(0, out pool))
					throw new Exception($"[NonAllocPoolWithAddress] NO POOL DETECTED AT ADDRESS MAX. DEPTH. LEVEL: {{ {level} }}");

				pool.Push(
					instance,
					dryRun);

				return;
			}

			int currentAddressHash = elementWithAddress.AddressHashes[level];

			if (!innerPoolsRepository.TryGet(currentAddressHash, out pool))
				throw new Exception($"[NonAllocPoolWithAddress] INVALID ADDRESS {{ {currentAddressHash} }}");

			pool.Push(
				instance,
				dryRun);
		}

		#endregion
	}
}