using Zenject;
using UnityEngine;
using System.Collections;
using HereticalSolutions.Messaging;
using HereticalSolutions.Collections;
using HereticalSolutions.Pools.Messages;

namespace HereticalSolutions.Pools
{
	public class PoolElementBehaviour : MonoBehaviour
	{
		[Inject(Id = "PoolBus")]
		MessageBus poolBus;

		[SerializeField]
		private string poolID;
		public string PoolID { get { return poolID; } }

		[SerializeField]
		private int MinResolveRequestTimeout = 0;

		[SerializeField]
		private int MaxResolveRequestTimeout = 3;


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
			StartCoroutine(TimeoutThenRequestResolveRoutine());
		}

		private IEnumerator TimeoutThenRequestResolveRoutine()
		{
			int timeout = UnityEngine.Random.Range(
				MinResolveRequestTimeout,
				MaxResolveRequestTimeout + 1);

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