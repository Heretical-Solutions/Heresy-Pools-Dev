using System.Threading;
using System.Runtime.CompilerServices;

namespace HereticalSolutions.Collections.Managed
{
    public class ConcurrentGenericCircularBuffer<TValue> where TValue : class
    {
        //Enum values stored in consts to quicken the state comparisons
        //We still need enum itself for serialization purposes
        private const int STATE_VACANT = (int)EBufferElementState.VACANT;
        
        private const int STATE_ALLOCATED_FOR_PRODUCER = (int)EBufferElementState.ALLOCATED_FOR_PRODUCER;
        
        private const int STATE_FILLED = (int)EBufferElementState.FILLED;
        
        private const int STATE_ALLOCATED_FOR_CONSUMER = (int)EBufferElementState.ALLOCATED_FOR_CONSUMER;
        
        
        
        private volatile TValue[] contents;

        private volatile int[] states;
        

        //private volatile int producerStart;

        private volatile int producerEnd;

        //private volatile int consumerStart;

        private volatile int consumerEnd;

        
        //public int ProducerStart { get { return Interlocked.CompareExchange(ref producerStart, 0, 0); } }

        public int ProducerEnd { get { return Interlocked.CompareExchange(ref producerEnd, 0, 0); } }

        //public int ConsumerStart { get { return Interlocked.CompareExchange(ref producerStart, 0, 0); } }

        public int ConsumerEnd { get { return Interlocked.CompareExchange(ref consumerEnd, 0, 0); } }


        public ConcurrentGenericCircularBuffer(
            TValue[] contents,
            int[] states)
        {
            this.contents = contents;

            this.states = states;
            
            //producerStart = 0;
            
            producerEnd = 0;

            //consumerStart = 0;

            consumerEnd = 0;
        }

        #region Get

        public EBufferElementState GetState(int index)
        {
            return (EBufferElementState)Interlocked.CompareExchange(ref states[index], 0, 0);
        }

        public TValue GetValue(int index)
        {
            return Interlocked.CompareExchange<TValue>(
                ref contents[index], 
                default(TValue), 
                default(TValue));
        }

        #endregion
        
        #region Produce

        public bool TryProduce(TValue value)
        {
            SpinWait spin = new SpinWait();

            return TryAllocateProducer(spin, out int index) && TryProduce(index, value);
        }

        private bool TryAllocateProducer(SpinWait spin, out int index)
        {
            int lastIndex = -1;

            while (true)
            {
                //Try claiming the slot at the end of the producer queue
                if (TryAllocateProducer(out index))
                    return true;

                //If we try to claim the same slot twice then the producer queue has met the consumer queue and the buffer is full
                if (index == lastIndex)
                {
                    index = -1;

                    return false;
                }

                //Otherwise it's some other thread that has snatched the slot, just spin and try claiming again
                lastIndex = index;
                
                spin.SpinOnce();
            }
        }

        private bool TryAllocateProducer(out int currentProducerEnd)
        {
            #region Read producer queue end index

            //Read the current producer queue end index
            currentProducerEnd = Interlocked.CompareExchange(ref producerEnd, 0, 0);

            #endregion
            
            #region Validate slot state

            //Read the state of the current producer queue end
            var currentState = Interlocked.CompareExchange(ref states[currentProducerEnd], 0, 0);
            
            //If it's not vacant then produce failure
            if (currentState != STATE_VACANT)
            {
                return false;
            }

            #endregion

            #region Claim the slot at the end of the queue

            //Try to write new descriptor values
            var updatedState = Interlocked.CompareExchange(
                    ref states[currentProducerEnd],
                    STATE_ALLOCATED_FOR_PRODUCER,
                    STATE_VACANT);
            
            //Proceed only if succeeded
            if (updatedState != currentState)
            {
                return false;
            }
            
            #endregion

            #region Increment producer queue end index and write back

            int newProducerEnd = IncrementAndWrap(currentProducerEnd);
            
            var updatedProducerEnd = Interlocked.CompareExchange(
                ref producerEnd,
                newProducerEnd,
                currentProducerEnd);

            #endregion
            
            return true;
        }

        private bool TryProduce(int index, TValue value)
        {
            //Write value
            Interlocked.Exchange<TValue>(
                ref contents[index],
                value);

            //Update state
            int updatedState = Interlocked.CompareExchange(
                ref states[index],
                STATE_FILLED,
                STATE_ALLOCATED_FOR_PRODUCER);

            return updatedState == STATE_ALLOCATED_FOR_PRODUCER;
        }
        
        #endregion

        #region Consume

        public bool TryConsume(out TValue value)
        {
            value = default(TValue);

            SpinWait spin = new SpinWait();

            return TryAllocateConsumer(spin, out int index) && TryConsume(index, out value);
        }

        private bool TryAllocateConsumer(SpinWait spin, out int index)
        {
            int lastIndex = -1;

            while (true)
            {
                //Try claiming the slot at the end of the consumer queue
                if (TryAllocateConsumer(out index))
                    return true;

                //If we try to claim the same slot twice then the consumer queue has met the producer queue and the buffer is empty
                if (index == lastIndex)
                {
                    index = -1;

                    return false;
                }

                //Otherwise it's some other thread that has snatched the slot, just spin and try claiming again
                lastIndex = index;
                
                spin.SpinOnce();
            }
        }

        private bool TryAllocateConsumer(out int currentConsumerEnd)
        {
            #region Read consumer queue end index

            //Read the current consumer queue end index
            currentConsumerEnd = Interlocked.CompareExchange(ref consumerEnd, 0, 0);

            #endregion
            
            #region Validate slot state

            //Read the state of the current consumer queue end
            var currentState = Interlocked.CompareExchange(ref states[currentConsumerEnd], 0, 0);
            
            //If it's not filled then produce failure
            if (currentState != STATE_FILLED)
            {
                return false;
            }

            #endregion

            #region Claim the slot at the end of the queue

            //Try to write new descriptor values
            var updatedState = Interlocked.CompareExchange(
                    ref states[currentConsumerEnd],
                    STATE_ALLOCATED_FOR_CONSUMER,
                    STATE_FILLED);
            
            //Proceed only if succeeded
            if (updatedState != currentState)
            {
                return false;
            }
            
            #endregion

            #region Increment consumer queue end index and write back

            int newConsumerEnd = IncrementAndWrap(currentConsumerEnd);
            
            var updatedConsumerEnd = Interlocked.CompareExchange(
                ref consumerEnd,
                newConsumerEnd,
                currentConsumerEnd);

            #endregion
            
            return true;
        }

        private bool TryConsume(int index, out TValue value)
        {
            //Read value
            value = Interlocked.CompareExchange<TValue>(
                ref contents[index],
                default(TValue),
                default(TValue));

            //Update state
            int updatedState = Interlocked.CompareExchange(
                ref states[index],
                STATE_VACANT,
                STATE_ALLOCATED_FOR_CONSUMER);

            return updatedState == STATE_ALLOCATED_FOR_CONSUMER;
        }

        #endregion
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int IncrementAndWrap(int index)
        {
            return (++index) % contents.Length;
        }
    }   
}