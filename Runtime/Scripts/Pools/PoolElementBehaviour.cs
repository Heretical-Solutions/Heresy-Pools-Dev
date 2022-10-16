using Zenject;
using UnityEngine;
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


		private INonAllocPool<GameObject> pool;

		private IPoolElement<GameObject> poolElement;


		private bool Initialized { get => poolElement != null; }


		public void Initialize(
			INonAllocPool<GameObject> pool,
			IPoolElement<GameObject> poolElement)
		{
			this.pool = pool;

			this.poolElement = poolElement;

			poolElement.Value = gameObject;
		}

		public void Push()
		{
			if (!Initialized)
				poolBus
					.PopMessage<ResolvePoolElementRequestMessage>(out var message)
					.Write(message, this)
					.SendImmediately(message);

			if (pool != null)
			{
				pool.Push(poolElement);
			}
			else
				Debug.LogError("[PrefabPoolInstance] INVALID POOL DEPENDENCY");
		}
	}
}