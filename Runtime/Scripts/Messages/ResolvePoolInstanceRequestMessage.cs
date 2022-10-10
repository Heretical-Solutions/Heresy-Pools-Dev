using HereticalSolutions.Messaging;

namespace HereticalSolutions.Pools.Messages
{
	public class ResolvePoolInstanceRequestMessage : IMessage
	{
		public PrefabPoolInstance Instance;

		public IMessage Write(params object[] args)
		{
			Instance = (PrefabPoolInstance)args[0];

			return this;
		}
	}
}
