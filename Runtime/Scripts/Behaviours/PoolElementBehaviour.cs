using Zenject;
using UnityEngine;
using System.Collections;
using HereticalSolutions.Messaging;
using HereticalSolutions.Collections;
using HereticalSolutions.Pools.Messages;
using HereticalSolutions.Pools.DAO;

namespace HereticalSolutions.Pools.Behaviours
{
	public class PoolElementBehaviour : MonoBehaviour
	{
		[Inject(Id = "PoolBus")]
		MessageBus poolBus;

		[SerializeField]
		private PoolElementRegistration registration;
		public PoolElementRegistration Registration { get { return registration; } }

		[SerializeField]
		private EResolutionBehaviour resolutionBehaviour;

		[SerializeField]
		private int MinResolveRequestTimeout = 1;

		[SerializeField]
		private int MaxResolveRequestTimeout = 1;


		private INonAllocDecoratedPool<GameObject> pool;

		private IPoolElement<GameObject> poolElement;


		public bool Initialized { get => poolElement != null; }

		public void Initialize(
			INonAllocDecoratedPool<GameObject> pool,
			IPoolElement<GameObject> poolElement)
		{
			this.pool = pool;

			this.poolElement = poolElement;
		}

		void Start()
		{
			switch (resolutionBehaviour)
			{
				case EResolutionBehaviour.IMMEDIATELY:
					RequestResolveIfNotInitialized();
					break;
				
				case EResolutionBehaviour.RESOLVE_AFTER_TICKS:
					StartCoroutine(TimeoutThenRequestResolveRoutine(MinResolveRequestTimeout));
					break;

				case EResolutionBehaviour.RESOLVE_AFTER_TICKS_IN_RANGE:
					int timeout = UnityEngine.Random.Range(
						MinResolveRequestTimeout,
						MaxResolveRequestTimeout + 1);

					StartCoroutine(TimeoutThenRequestResolveRoutine(timeout));
					break;
			}
		}

		private IEnumerator TimeoutThenRequestResolveRoutine(int timeout)
		{
			for (int i = 0; i < timeout; i++)
				yield return null;

			RequestResolveIfNotInitialized();
		}

		private void RequestResolveIfNotInitialized()
		{
			if (!Initialized)
				poolBus
					.PopMessage<ResolvePoolElementRequestMessage>(out var message)
					.Write(message, this)
					.SendImmediately(message);
		}

		public void Push()
		{
			RequestResolveIfNotInitialized();

			if (pool != null)
			{
				pool.Push(poolElement);
			}
			else
				Debug.LogError("[PoolElementBehaviour] INVALID POOL DEPENDENCY");
		}
	}
}