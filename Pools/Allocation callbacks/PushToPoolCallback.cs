namespace HereticalSolutions.Pools.AllocationCallbacks
{
    public class PushToPoolCallback<T> : IAllocationCallback<T>
    {
        private INonAllocPool<T> root; 
        public INonAllocPool<T> Root
        {
            get => root;
            set
            {
                root = value;

                if (deferredCallbackQueue != null)
                {
                    deferredCallbackQueue.Process();
                    
                    deferredCallbackQueue.Callback = null;

                    deferredCallbackQueue = null;
                }
            }
        }

        private DeferredCallbackQueue<T> deferredCallbackQueue;

        public PushToPoolCallback(INonAllocPool<T> root = null)
        {
            this.root = root;

            deferredCallbackQueue = null;
        }
        
        public PushToPoolCallback(DeferredCallbackQueue<T> deferredCallbackQueue)
        {
            root = null;
            
            this.deferredCallbackQueue = deferredCallbackQueue;

            this.deferredCallbackQueue.Callback = this;
        }

        public void OnAllocated(IPoolElement<T> currentElement)
        {
            if (currentElement.Value == null)
                return;

            if (root == null)
            {
                deferredCallbackQueue?.Enqueue(currentElement);

                return;
            }

            root.Push(currentElement);
        }
    }
}