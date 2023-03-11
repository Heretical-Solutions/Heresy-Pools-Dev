namespace HereticalSolutions.Pools
{
    public interface IPushBehaviourHandler<T>
    {
        void Push(IPoolElement<T> poolElement);
    }
}