namespace HereticalSolutions.Pools.Behaviours
{
    public class PushToINonAllocPoolBehaviour<T> : IPushBehaviourHandler<T>
    {
        private readonly INonAllocPool<T> pool;

        public PushToINonAllocPoolBehaviour(INonAllocPool<T> pool)
        {
            this.pool = pool;
        }

        public void Push(IPoolElement<T> poolElement)
        {
            pool.Push(poolElement);
        }
    }
}