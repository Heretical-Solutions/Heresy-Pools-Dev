namespace HereticalSolutions.Collections
{
	public interface IPool<T>
	{
		T Pop();

		void Push(T instance);
		
		bool HasFreeSpace { get; }
	}
}