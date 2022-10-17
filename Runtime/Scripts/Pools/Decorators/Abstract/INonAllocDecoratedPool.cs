using HereticalSolutions.Collections;
using HereticalSolutions.Pools.Arguments;

namespace HereticalSolutions.Pools
{
	public interface INonAllocDecoratedPool<T>
	{
		IPoolElement<T> Pop(params IPoolDecoratorArgument[] args);

		void Push(IPoolElement<T> instance);
	}
}