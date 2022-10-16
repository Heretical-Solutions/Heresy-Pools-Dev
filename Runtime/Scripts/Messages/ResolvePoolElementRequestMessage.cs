using HereticalSolutions.Messaging;

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