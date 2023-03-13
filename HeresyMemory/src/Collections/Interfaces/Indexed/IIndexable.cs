namespace HereticalSolutions.Collections
{
	public interface IIndexable<T>
	{
		int Count { get; }

		T this[int index] { get; }

		T Get(int index);
	}
}