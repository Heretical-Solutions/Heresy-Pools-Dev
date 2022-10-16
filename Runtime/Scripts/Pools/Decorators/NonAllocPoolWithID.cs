using HereticalSolutions.Collections;

namespace HereticalSolutions.Pools
{
	public class NonAllocPoolWithID<T> : ANonAllocDecoratorPool<T>
	{
		public string ID { get; private set; }

		public NonAllocPoolWithID(
			INonAllocPool<T> innerPool,
			string id)
			: base(innerPool)
		{
			ID = id;
		}
	}
}