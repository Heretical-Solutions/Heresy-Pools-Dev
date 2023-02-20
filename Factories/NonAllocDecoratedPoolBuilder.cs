namespace HereticalSolutions.Pools.Factories
{
	public class NonAllocDecoratedPoolBuilder<T>
	{
		public INonAllocDecoratedPool<T> CurrentWrapper { get; private set; }

		public NonAllocDecoratedPoolBuilder<T> Add(INonAllocDecoratedPool<T> newWrapper)
		{
			CurrentWrapper = newWrapper;

			return this;
		}

		public NonAllocDecoratedPoolBuilder<T> Add(
			INonAllocDecoratedPool<T> newWrapper,
			out INonAllocDecoratedPool<T> wrapperOut)
		{
			CurrentWrapper = newWrapper;

			wrapperOut = newWrapper;

			return this;
		}
	}
}