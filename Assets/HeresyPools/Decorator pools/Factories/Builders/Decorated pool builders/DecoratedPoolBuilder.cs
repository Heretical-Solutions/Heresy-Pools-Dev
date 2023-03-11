namespace HereticalSolutions.Pools.Factories
{
    public class DecoratedPoolBuilder<T>
    {
        public IDecoratedPool<T> CurrentWrapper { get; private set; }

        public DecoratedPoolBuilder<T> Add(IDecoratedPool<T> newWrapper)
        {
            CurrentWrapper = newWrapper;

            return this;
        }

        public DecoratedPoolBuilder<T> Add(
            IDecoratedPool<T> newWrapper,
            out IDecoratedPool<T> wrapperOut)
        {
            CurrentWrapper = newWrapper;

            wrapperOut = newWrapper;

            return this;
        }
    }
}