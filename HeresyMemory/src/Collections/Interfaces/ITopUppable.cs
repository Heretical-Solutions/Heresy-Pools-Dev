namespace HereticalSolutions.Collections
{
	public interface ITopUppable<T>
	{
		void TopUp(IPoolElement<T> value);
	}
}