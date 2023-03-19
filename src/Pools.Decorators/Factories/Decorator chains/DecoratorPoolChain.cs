namespace HereticalSolutions.Pools.Factories
{
    public class DecoratorPoolChain<T>
    {
        public IDecoratedPool<T> TopWrapper { get; private set; }

        public DecoratorPoolChain<T> Add(IDecoratedPool<T> newWrapper)
        {
            TopWrapper = newWrapper;

            return this;
        }

        public DecoratorPoolChain<T> Add(
            IDecoratedPool<T> newWrapper,
            out IDecoratedPool<T> wrapperOut)
        {
            TopWrapper = newWrapper;

            wrapperOut = newWrapper;

            return this;
        }
    }
}