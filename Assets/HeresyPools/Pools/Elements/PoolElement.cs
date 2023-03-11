using HereticalSolutions.Repositories;

namespace HereticalSolutions.Pools.Elements
{
    public class PoolElement<T>
        : IPoolElement<T>,
          IPushable<T>
    {
        public PoolElement(
            T defaultValue,
            IReadOnlyObjectRepository metadata)
        {
            Value = defaultValue;
            
            status = EPoolElementStatus.UNINITIALIZED;

            this.metadata = metadata;

            pushBehaviourHandler = null;
        }

        #region IPoolElement
        
        #region Value

        public T Value { get; set; }

        #endregion
        
        #region Status
        
        private EPoolElementStatus status;
        
        public EPoolElementStatus Status
        {
            get => status;
            set => status = value;
        }
        
        #endregion

        #region Metadata
        
        private readonly IReadOnlyObjectRepository metadata;
        
        public IReadOnlyObjectRepository Metadata
        {
            get => metadata;
        }
        
        #endregion

        #region Push
        
        private IPushBehaviourHandler<T> pushBehaviourHandler;

        public void Push()
        {
            if (status == EPoolElementStatus.PUSHED)
                return;
            
            pushBehaviourHandler?.Push(this);
        }
        
        #endregion
        
        #endregion

        #region IPushable

        public void UpdatePushBehaviour(IPushBehaviourHandler<T> pushBehaviourHandler)
        {
            this.pushBehaviourHandler = pushBehaviourHandler;
        }

        #endregion
    }
}