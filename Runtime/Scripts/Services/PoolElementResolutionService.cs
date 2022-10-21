using System;

using UnityEngine;

using HereticalSolutions.Messaging;
using HereticalSolutions.Repositories;

using HereticalSolutions.Pools.Arguments;
using HereticalSolutions.Pools.Messages;
using HereticalSolutions.Pools.Behaviours;

using UniRx;

namespace HereticalSolutions.Pools.Services
{
	public class PoolElementResolutionService
	{
		private MessageBus poolBus;

		private CompositeDisposable disposables;

		private IRepository<string, INonAllocDecoratedPool<GameObject>> poolRepository;

		private AppendArgument appendArgument = new AppendArgument();

		private AddressArgument addressArgument = new AddressArgument();

		private VariantArgument variantArgument = new VariantArgument();

		private ArgumentBuilderNonAlloc argumentBuilder = new ArgumentBuilderNonAlloc(new IPoolDecoratorArgument[5]);

		public PoolElementResolutionService(
			MessageBus poolBus,
			IRepository<string, INonAllocDecoratedPool<GameObject>> poolRepository)
		{
			this.poolBus = poolBus;

			this.poolRepository = poolRepository;

			disposables = new CompositeDisposable();

			poolBus.SubscribeTo<ResolvePoolElementRequestMessage>(@event =>
            {
                Resolve(@event.Instance);
            })
            .AddTo(disposables);
		}

		private void Resolve(PoolElementBehaviour behaviour)
		{
			if (!poolRepository.TryGet(behaviour.Registration.PoolID, out var pool))
				throw new Exception($"[PoolElementResolutionService] COULD NOT FIND POOL BY ID {{ {behaviour.Registration.PoolID} }}");

			var builder = argumentBuilder
				.Clean()
				.Add(appendArgument);

			if (behaviour.Registration.Address != null
				&& behaviour.Registration.Address.Length > 0)
			{
				addressArgument.Address = behaviour.Registration.Address;

				builder.Add(addressArgument);
			}

			if (behaviour.Registration.Variant != -1)
			{
				variantArgument.Variant = behaviour.Registration.Variant;

				builder.Add(variantArgument);
			}

			var element = pool.Pop(builder.Build());

			element.Value = behaviour.gameObject;
		}
	}
}