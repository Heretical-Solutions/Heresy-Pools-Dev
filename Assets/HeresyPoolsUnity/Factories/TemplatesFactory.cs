using System;

using HereticalSolutions.Collections.Allocations;

using HereticalSolutions.Pools.AllocationCallbacks;
using HereticalSolutions.Pools.Allocations;
using HereticalSolutions.Pools.Decorators;

using UnityEngine;

namespace HereticalSolutions.Pools.Factories
{
    public static partial class PoolsFactory
    {
        #region Templates

	    public static INonAllocDecoratedPool<GameObject> BuildSimpleGameObjectPool(
	        string ID,
	        GameObject prefab,
	        Transform poolParent,
	        AllocationCommandDescriptor initialAllocation,
	        AllocationCommandDescriptor additionalAllocation)
        {
	        ResizablePoolBuilder<GameObject> resizablePoolBuilder = new ResizablePoolBuilder<GameObject>();
	        
	        #region Value allocation delegate initialization

	        Func<GameObject> valueAllocationDelegate =
		        () => PoolsFactory.InstantiateAllocationDelegate(prefab);

	        #endregion

	        #region Metadata initialization

	        var metadataDescriptorBuilders = new Func<MetadataAllocationDescriptor>[]
	        {
		        PoolsFactory.BuildIndexedMetadataDescriptor
	        };

	        #endregion

	        #region Allocation callbacks initialization

	        RenameByStringAndIndexCallback renameCallback = PoolsFactory.BuildRenameByStringAndIndexCallback(ID);
	        PushToDecoratedPoolCallback<GameObject> pushCallback =
		        PoolsFactory.BuildPushToDecoratedPoolCallback<GameObject>(
			        PoolsFactory.BuildDeferredCallbackQueue<GameObject>());

	        var callbacks = new IAllocationCallback<GameObject>[]
	        {
		        renameCallback,
		        pushCallback
	        };

	        #endregion
	        
	        #region Resizable pool builder initialization

	        resizablePoolBuilder.Initialize(
		        valueAllocationDelegate,
		        metadataDescriptorBuilders,
		        initialAllocation,
		        additionalAllocation,
		        callbacks);
	        
	        #endregion

	        #region Decorator pools initialization

	        var decoratorChain = new NonAllocDecoratorPoolChain<GameObject>();

	        decoratorChain
		        .Add(resizablePoolBuilder.Build())
		        .Add(PoolsFactory.BuildNonAllocGameObjectPool(decoratorChain.TopWrapper, poolParent))
		        .Add(PoolsFactory.BuildNonAllocPrefabInstancePool(decoratorChain.TopWrapper, prefab))
		        .Add(PoolsFactory.BuildNonAllocPoolWithID<GameObject>(decoratorChain.TopWrapper, ID));

	        var result = decoratorChain.TopWrapper;

	        #endregion

	        #region Dependency injection

	        pushCallback.Root = result;

	        #endregion

	        return result;
        }

        #endregion
    }
}