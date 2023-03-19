namespace HereticalSolutions.Pools.Factories
{
	public class NonAllocDecoratorPoolChain<T>
	{
		public INonAllocDecoratedPool<T> TopWrapper { get; private set; }

		public NonAllocDecoratorPoolChain<T> Add(INonAllocDecoratedPool<T> newWrapper)
		{
			TopWrapper = newWrapper;

			return this;
		}

		public NonAllocDecoratorPoolChain<T> Add(
			INonAllocDecoratedPool<T> newWrapper,
			out INonAllocDecoratedPool<T> wrapperOut)
		{
			TopWrapper = newWrapper;

			wrapperOut = newWrapper;

			return this;
		}
	}
}