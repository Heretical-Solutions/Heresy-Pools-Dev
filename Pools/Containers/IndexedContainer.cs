using HereticalSolutions.Collections;

namespace HereticalSolutions.Pools
{
	public class IndexedContainer<T> 
		: IPoolElement<T>,
		  IIndexed
	{
		public int Index { get; set; }

		public T Value { get; set; } = default(T);

		public IndexedContainer(
			T initialValue)
		{
			Value = initialValue;

			Index = -1;
		}
	}
}