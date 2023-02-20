using HereticalSolutions.Collections;
using HereticalSolutions.Collections.Managed;

namespace HereticalSolutions.Pools
{
	public interface IValueAssignedNotifiable<T>
	{
		void Notify(IPoolElement<T> element);
	}
}