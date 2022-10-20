using System;
using HereticalSolutions.Collections;
using HereticalSolutions.Repositories;
using HereticalSolutions.Pools.Arguments;

namespace HereticalSolutions.Pools
{
	public class CompositePoolWithAddresses<T>
		: INonAllocDecoratedPool<T>
	{
		private int level;

		private IRepository<string, INonAllocDecoratedPool<T>> poolsRepository;

		public CompositePoolWithAddresses(
			IRepository<string, INonAllocDecoratedPool<T>> poolsRepository,
			int level)
		{
			this.poolsRepository = poolsRepository;

			this.level = level;
		}

		#region Pop

		public IPoolElement<T> Pop(IPoolDecoratorArgument[] args)
		{
			if (!args.TryGetArgument<AddressArgument>(out var arg))
				throw new Exception("[CompositePoolWithAddresses] ADDRESS ARGUMENT ABSENT");

			if (arg.Address.Length <= level)
				throw new Exception($"[CompositePoolWithAddresses] INVALID ADDRESS DEPTH. LEVEL: {{ {level} }} ADDRESS LENGTH: {{ {arg.Address.Length} }}");

			string currentAddress = arg.Address[level];

			if (!poolsRepository.TryGet(currentAddress, out var pool))
				throw new Exception($"[CompositePoolWithAddresses] INVALID ADDRESS {{ {currentAddress} }}");

			var result = pool.Pop(args);

			return result;
		}

		#endregion

		#region Push

		public void Push(
			IPoolElement<T> instance,
			bool dryRun = false)
		{
			var elementWithAddress = (IAddressContainable)instance;

			if (elementWithAddress == null)
				throw new Exception("[CompositePoolWithAddresses] INVALID INSTANCE");

			if (!poolsRepository.TryGet(elementWithAddress.Address, out var pool))
				throw new Exception($"[CompositePoolWithAddresses] INVALID ADDRESS {{ {elementWithAddress.Address} }}");

			pool.Push(
				instance,
				dryRun);
		}

		#endregion
	}
}