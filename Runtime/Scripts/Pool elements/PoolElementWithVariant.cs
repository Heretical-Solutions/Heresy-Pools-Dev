using HereticalSolutions.Collections;
using HereticalSolutions.Collections.Managed;

namespace HereticalSolutions.Pools
{
	public class PoolElementWithVariant<T>
		: IPoolElement<T>,
		  IIndexed
	{
		public int Index { get; set; }

		public T Value { get; set; } = default(T);

		public int Variant { get; set; }

		public PoolElementWithVariant(
			T initialValue)
		{
			Value = initialValue;

			Index = -1;

			Variant = -1;
		}
	}
}