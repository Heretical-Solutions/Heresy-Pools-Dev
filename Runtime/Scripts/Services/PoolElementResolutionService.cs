using System;
using System.Collections.Generic;

using UnityEngine;

using HereticalSolutions.Messaging;
using HereticalSolutions.Collections;
using HereticalSolutions.Allocations;
using HereticalSolutions.Repositories;

using HereticalSolutions.Pools.Arguments;
using HereticalSolutions.Pools.Messages;

using Zenject;
using UniRx;

namespace HereticalSolutions.Pools.Services
{
	public class PoolElementResolutionService
	{
		private MessageBus poolBus;

		private CompositeDisposable disposables;

		private IRepository<string, INonAllocDecoratedPool<GameObject>> poolRepository;

		private IPoolDecoratorArgument[] argumentsCache;

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

			argumentsCache = new ArgumentBuilder()
				.Add<AppendArgument>(out var argument)
				.Build();
		}

		private void Resolve(PoolElementBehaviour behaviour)
		{
			if (!poolRepository.TryGet(behaviour.PoolID, out var pool))
				throw new Exception($"[PoolElementResolutionService] COULD NOT FIND POOL BY ID {{ {behaviour.PoolID} }}");

			var element = pool.Pop(argumentsCache);

			element.Value = behaviour.gameObject;
		}
	}
}