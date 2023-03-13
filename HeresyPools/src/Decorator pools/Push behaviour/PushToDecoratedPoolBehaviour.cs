namespace HereticalSolutions.Pools.Behaviours
{
    public class PushToDecoratedPoolBehaviour<T> : IPushBehaviourHandler<T>
    {
        private readonly INonAllocDecoratedPool<T> pool;

        public PushToDecoratedPoolBehaviour(INonAllocDecoratedPool<T> pool)
        {
            this.pool = pool;
        }

        public void Push(IPoolElement<T> poolElement)
        {
            pool.Push(poolElement, false);
        }
    }
}