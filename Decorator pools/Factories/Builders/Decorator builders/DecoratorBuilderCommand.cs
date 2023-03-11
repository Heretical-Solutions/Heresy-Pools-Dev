using System;
using System.Collections.Generic;

using HereticalSolutions.Collections.Allocations;
using HereticalSolutions.Pools.Allocations;

using UnityEngine;

using Zenject;

namespace HereticalSolutions.Pools.Factories
{
    public class DecoratorBuilderCommand<T>
    {
        #region Context specific
        
        //Identity
        public string ID;
        
        //DI
        public DiContainer Container;
        
        //GameObject stuff
        public GameObject Prefab;
	        
        #endregion
        
        //Metadata
        public List<Func<MetadataAllocationDescriptor>> MetadataDescriptorBuilders { get; private set; }

        //Allocation
        public AllocationCommandDescriptor InitialAllocation;
        public AllocationCommandDescriptor AdditionalAllocation;
        public List<IAllocationCallback<T>> Callbacks { get; private set; }
    }
}