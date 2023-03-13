namespace HereticalSolutions.Pools.Decorators
{
	public class PoolWithID<T> : ADecoratorPool<T>
	{
		public string ID { get; private set; }

		public PoolWithID(
			IDecoratedPool<T> innerPool,
			string id)
			: base(innerPool)
		{
			ID = id;
		}
	}
}