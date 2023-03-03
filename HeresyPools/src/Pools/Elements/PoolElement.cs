using HeresyPools.Pools;

namespace HereticalSolutions.Pools.Elements
{
    public class PoolElement<T> : IPoolElement<T>
    {
        public T Value { get; set; }

        private INonAllocPool<T> pool;

        private EPoolElementStatus status;
        
        public EPoolElementStatus Status
        {
            get => status;
        }
        
        private IMetadata metadata;
        
        public IMetadata Metadata
        {
            get => metadata;
        }
        
        public void Push()
        {
            
        }
    }
}