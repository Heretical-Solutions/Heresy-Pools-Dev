using System;
using System.Collections.Generic;

using HereticalSolutions.Collections.Allocations;

using HereticalSolutions.Pools.Allocations;

namespace HereticalSolutions.Pools.Factories
{
    public class ResizablePoolBuilder<T>
    {
        private Func<T> valueAllocationDelegate;
        
        private Func<MetadataAllocationDescriptor>[] metadataDescriptorBuilders;

        private AllocationCommandDescriptor initialAllocation;
        
        private AllocationCommandDescriptor additionalAllocation;
        
        private IAllocationCallback<T>[] callbacks;
        
        public void Initialize(
            Func<T> valueAllocationDelegate,
            Func<MetadataAllocationDescriptor>[] metadataDescriptorBuilders,
            AllocationCommandDescriptor initialAllocation,
            AllocationCommandDescriptor additionalAllocation,
            IAllocationCallback<T>[] callbacks)
        {
            this.valueAllocationDelegate = valueAllocationDelegate;

            this.metadataDescriptorBuilders = metadataDescriptorBuilders;

            this.initialAllocation = initialAllocation;

            this.additionalAllocation = additionalAllocation;

            this.callbacks = callbacks;
        }

        public INonAllocDecoratedPool<T> Build()
        {
            if (valueAllocationDelegate == null)
                throw new Exception("[ResizablePoolBuilder] BUILDER NOT INITIALIZED");

            #region Metadata initialization

            List<MetadataAllocationDescriptor> metadataDescriptorsList = new List<MetadataAllocationDescriptor>();

            foreach (var descriptorBuilder in metadataDescriptorBuilders)
            {
                if (descriptorBuilder != null)
                    metadataDescriptorsList.Add(descriptorBuilder());
            }

            var metadataDescriptors = metadataDescriptorsList.ToArray();

            #endregion
            
            #region Allocation callbacks initialization
	        
            IAllocationCallback<T> callback = PoolsFactory.BuildCompositeCallback(
                callbacks);

            #endregion
            
            INonAllocDecoratedPool<T> result = PoolsFactory.BuildResizableNonAllocPoolWithAllocationCallback(
                valueAllocationDelegate,
                metadataDescriptors,
                initialAllocation,
                additionalAllocation,
                callback);

            valueAllocationDelegate = null;

            metadataDescriptorBuilders = null;

            initialAllocation = default(AllocationCommandDescriptor);

            additionalAllocation = default(AllocationCommandDescriptor);

            callbacks = null;

            return result;
        }
    }
}