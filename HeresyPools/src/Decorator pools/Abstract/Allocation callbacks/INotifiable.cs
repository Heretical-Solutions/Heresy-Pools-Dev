using HereticalSolutions.Collections;
using HereticalSolutions.Collections.Managed;

namespace HereticalSolutions.Pools
{
	public interface INotifiable<T>
	{
		void Notify(IPoolElement<T> element);
	}
}