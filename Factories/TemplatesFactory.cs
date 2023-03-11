using System;

using HereticalSolutions.Collections.Allocations;

using HereticalSolutions.Pools.AllocationCallbacks;
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
	        #region Value allocation delegate initialization

	        Func<GameObject> valueAllocationDelegate =
		        () => PoolsFactory.InstantiateAllocationDelegate(prefab);

	        #endregion

	        #region Metadata initialization

	        var metadataDescriptors = new[]
	        {
		        PoolsFactory.BuildIndexedMetadataDescriptor()
	        };

	        #endregion

	        #region Allocation callbacks initialization

	        RenameByStringAndIndexCallback renameCallback = PoolsFactory.BuildRenameByStringAndIndexCallback(ID);
	        PushToDecoratedPoolCallback<GameObject> pushCallback =
		        PoolsFactory.BuildPushToDecoratedPoolCallback<GameObject>(
			        PoolsFactory.BuildDeferredCallbackQueue<GameObject>());

	        IAllocationCallback<GameObject> callback = PoolsFactory.BuildCompositeCallback(
		        new IAllocationCallback<GameObject>[]
		        {
			        renameCallback,
			        pushCallback
		        });

	        #endregion

	        #region Resizable pool initialization

	        INonAllocDecoratedPool<GameObject> nonAllocPool = PoolsFactory.BuildResizableNonAllocPoolWithAllocationCallback(
		        valueAllocationDelegate,
		        metadataDescriptors,
		        initialAllocation,
		        additionalAllocation,
		        callback);

	        #endregion

	        #region Decorator pools initialization

	        var builder = new NonAllocDecoratorPoolChain<GameObject>();

	        builder
		        .Add(new NonAllocGameObjectPool(builder.TopWrapper, poolParent))
		        .Add(new NonAllocPrefabInstancePool(builder.TopWrapper, prefab))
		        .Add(new NonAllocPoolWithID<GameObject>(builder.TopWrapper, ID));

	        var result = builder.TopWrapper;

	        #endregion

	        #region Dependency injection

	        pushCallback.Root = result;

	        #endregion

	        return result;
        }

        #endregion
    }
}