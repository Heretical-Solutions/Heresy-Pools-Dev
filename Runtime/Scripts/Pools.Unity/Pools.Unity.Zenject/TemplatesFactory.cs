using System;

using HereticalSolutions.Collections.Allocations;

using HereticalSolutions.Pools.AllocationCallbacks;
using HereticalSolutions.Pools.Decorators;

using UnityEngine;

using Zenject;

namespace HereticalSolutions.Pools.Factories
{
    public static partial class PoolsFactory
    {
        #region Templates

        public static INonAllocDecoratedPool<GameObject> BuildSimpleGameObjectPool(
	        DiContainer container,
	        string ID,
	        GameObject prefab,
	        Transform poolParent,
	        AllocationCommandDescriptor initialAllocation,
	        AllocationCommandDescriptor additionalAllocation)
        {
	        #region Value allocation delegate initialization

	        Func<GameObject> valueAllocationDelegate =
		        () => PoolsFactory.DIResolveOrInstantiateAllocationDelegate(container, prefab);

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

	        INonAllocPool<GameObject> nonAllocPool = PoolsFactory.BuildResizableNonAllocPoolWithAllocationCallback(
		        valueAllocationDelegate,
		        metadataDescriptors,
		        initialAllocation,
		        additionalAllocation,
		        callback);

	        #endregion

	        #region Decorator pools initialization

	        var builder = new NonAllocDecoratedPoolBuilder<GameObject>();

	        builder
		        .Add(new NonAllocDecoratorPool<GameObject>(nonAllocPool))
		        .Add(new NonAllocGameObjectPool(builder.CurrentWrapper, poolParent))
		        .Add(new NonAllocPrefabInstancePool(builder.CurrentWrapper, prefab))
		        .Add(new NonAllocPoolWithID<GameObject>(builder.CurrentWrapper, ID));

	        var result = builder.CurrentWrapper;

	        #endregion

	        #region Dependency injection

	        pushCallback.Root = result;

	        #endregion

	        return result;
        }

        #endregion
    }
}