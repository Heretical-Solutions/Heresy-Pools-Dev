namespace HereticalSolutions.Pools.Decorators
{
	public class NonAllocPoolWithID<T> : ANonAllocDecoratorPool<T>
	{
		public string ID { get; private set; }

		public NonAllocPoolWithID(
			INonAllocDecoratedPool<T> innerPool,
			string id)
			: base(innerPool)
		{
			ID = id;
		}
	}
}