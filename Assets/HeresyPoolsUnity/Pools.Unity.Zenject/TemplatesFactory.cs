using System;
using System.Collections.Generic;
using HereticalSolutions.Collections.Allocations;

using HereticalSolutions.Pools.AllocationCallbacks;
using HereticalSolutions.Pools.Allocations;
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
	        string id,
	        GameObject prefab,
	        Transform poolParent,
	        AllocationCommandDescriptor initialAllocation,
	        AllocationCommandDescriptor additionalAllocation)
        {
	        #region Metadata initialization

	        var metadataDescriptorBuilders = new Func<MetadataAllocationDescriptor>[]
	        {
		        PoolsFactory.BuildIndexedMetadataDescriptor
	        };

	        #endregion

	        #region Allocation callbacks initialization

	        RenameByStringAndIndexCallback renameCallback = PoolsFactory.BuildRenameByStringAndIndexCallback(id);
	        
	        PushToDecoratedPoolCallback<GameObject> pushCallback =
		        PoolsFactory.BuildPushToDecoratedPoolCallback<GameObject>(
			        PoolsFactory.BuildDeferredCallbackQueue<GameObject>());

	        var callbacks = new IAllocationCallback<GameObject>[]
	        {
		        renameCallback,
		        pushCallback
	        };

	        #endregion

	        #region Resizable pool initialization

	        BuildGameObjectPoolCommand command = new BuildGameObjectPoolCommand
	        {
		        Container = container,
				ID = id,
		        MetadataDescriptorBuilders = metadataDescriptorBuilders,
				Prefab = prefab,
				InitialAllocation = initialAllocation,
				AdditionalAllocation = additionalAllocation,
				Callbacks = callbacks
	        };

	        #endregion

	        #region Decorator pools initialization

	        var builder = new NonAllocDecoratorPoolChain<GameObject>();

	        builder
		        .Add(BuildResizableGameObjectPool(command))
		        .Add(new NonAllocGameObjectPool(builder.TopWrapper, poolParent))
		        .Add(new NonAllocPrefabInstancePool(builder.TopWrapper, prefab))
		        .Add(new NonAllocPoolWithID<GameObject>(builder.TopWrapper, id));

	        var result = builder.TopWrapper;

	        #endregion

	        #region Dependency injection

	        pushCallback.Root = result;

	        #endregion

	        return result;
        }

        private class BuildGameObjectPoolCommand
        {
	        //DI
	        public DiContainer Container;
	        
	        //Identity
	        public string ID;
	        public Func<MetadataAllocationDescriptor>[] MetadataDescriptorBuilders;
	        
	        //GameObject stuff
	        public GameObject Prefab;
	        
	        //Allocation
	        public AllocationCommandDescriptor InitialAllocation;
	        public AllocationCommandDescriptor AdditionalAllocation;
	        public IAllocationCallback<GameObject>[] Callbacks;
        }

        private static INonAllocDecoratedPool<GameObject> BuildResizableGameObjectPool(BuildGameObjectPoolCommand buildCommand)
        {
	        #region Value allocation delegate initialization

	        Func<GameObject> valueAllocationDelegate =
		        () => PoolsFactory.DIResolveOrInstantiateAllocationDelegate(
			        buildCommand.Container,
			        buildCommand.Prefab);

	        #endregion

	        #region Metadata initialization

	        List<MetadataAllocationDescriptor> metadataDescriptorsList = new List<MetadataAllocationDescriptor>();

	        foreach (var descriptorBuilder in buildCommand.MetadataDescriptorBuilders)
	        {
		        if (descriptorBuilder != null)
					metadataDescriptorsList.Add(descriptorBuilder());
	        }

	        var metadataDescriptors = metadataDescriptorsList.ToArray();

	        #endregion

	        #region Allocation callbacks initialization
	        
	        IAllocationCallback<GameObject> callback = PoolsFactory.BuildCompositeCallback(
		        buildCommand.Callbacks);

	        #endregion

	        #region Resizable pool initialization

	        INonAllocDecoratedPool<GameObject> nonAllocPool = PoolsFactory.BuildResizableNonAllocPoolWithAllocationCallback(
		        valueAllocationDelegate,
		        metadataDescriptors,
		        buildCommand.InitialAllocation,
		        buildCommand.AdditionalAllocation,
		        callback);

	        #endregion
	        
	        return nonAllocPool;
        }

        #endregion
    }
}