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
	        
	        INonAllocPool<GameObject> nonAllocPool = BuildGameObjectPool(command);

	        #endregion

	        #region Decorator pools initialization

	        var builder = new NonAllocDecoratedPoolBuilder<GameObject>();

	        builder
		        .Add(new NonAllocDecoratorPool<GameObject>(nonAllocPool))
		        .Add(new NonAllocGameObjectPool(builder.CurrentWrapper, poolParent))
		        .Add(new NonAllocPrefabInstancePool(builder.CurrentWrapper, prefab))
		        .Add(new NonAllocPoolWithID<GameObject>(builder.CurrentWrapper, id));

	        var result = builder.CurrentWrapper;

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

        /*
        private class BuildGameObjectPoolResult
        {
	        public INonAllocPool<GameObject> Result;
	        public PushToDecoratedPoolCallback<GameObject> PushCallback;
        }
        */

        private static INonAllocPool<GameObject> BuildGameObjectPool(BuildGameObjectPoolCommand buildCommand)
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

	        INonAllocPool<GameObject> nonAllocPool = PoolsFactory.BuildResizableNonAllocPoolWithAllocationCallback(
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