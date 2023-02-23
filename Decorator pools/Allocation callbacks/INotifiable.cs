namespace HereticalSolutions.Pools
{
	public interface INotifiable<T>
	{
		void Notify(IPoolElement<T> element);
	}
}