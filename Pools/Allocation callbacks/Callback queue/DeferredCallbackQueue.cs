using System.Collections.Generic;

namespace HereticalSolutions.Pools.AllocationCallbacks
{
    public class DeferredCallbackQueue<T>
    {
        private Queue<IPoolElement<T>> elementsQueue;

        public IAllocationCallback<T> Callback { get; set; }

        public DeferredCallbackQueue()
        {
            elementsQueue = new Queue<IPoolElement<T>>();
        }

        public void Enqueue(IPoolElement<T> element)
        {
            elementsQueue.Enqueue(element);
        }

        public void Process()
        {
            while (elementsQueue.Count > 0)
            {
                Callback.OnAllocated(elementsQueue.Dequeue());
            }
        }
    }
}