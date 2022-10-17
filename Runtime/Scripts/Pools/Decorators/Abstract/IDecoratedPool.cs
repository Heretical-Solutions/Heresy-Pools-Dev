using HereticalSolutions.Pools.Arguments;

namespace HereticalSolutions.Pools
{
	public interface IDecoratedPool<T>
	{
		T Pop(params IPoolDecoratorArgument[] args);

		void Push(T instance);
	}
}