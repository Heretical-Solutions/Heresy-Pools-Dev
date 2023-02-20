using HereticalSolutions.Messaging;
using HereticalSolutions.Pools.Behaviours;

namespace HereticalSolutions.Pools.Messages
{
	public class ResolvePoolElementRequestMessage : IMessage
	{
		public PoolElementBehaviour Instance;

		public void Write(params object[] args)
		{
			Instance = (PoolElementBehaviour)args[0];
		}
	}
}