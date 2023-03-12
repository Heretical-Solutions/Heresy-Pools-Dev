using System;

using HereticalSolutions.Pools.AllocationCallbacks;
using HereticalSolutions.Pools.Allocations;
using HereticalSolutions.Pools.Decorators;

using UnityEngine;

using Zenject;

namespace HereticalSolutions.Pools.Factories
{
    public static partial class PoolsFactory
    {
        #region Samples

        public static INonAllocDecoratedPool<GameObject> BuildPool(
            DiContainer container,
            PoolSettings settings,
            Transform parentTransform = null)
        {
            #region Builders

            var poolWithAddressBuilder = new PoolWithAddressBuilder<GameObject>();
            
            var poolWithVariantsBuilder = new PoolWithVariantsBuilder<GameObject>();

            var resizablePoolBuilder = new ResizablePoolBuilder<GameObject>();
            
            #endregion

            #region Callbacks
            
            PushToDecoratedPoolCallback<GameObject> pushCallback =
                PoolsFactory.BuildPushToDecoratedPoolCallback<GameObject>(
                    PoolsFactory.BuildDeferredCallbackQueue<GameObject>());
            
            #endregion
            
            #region Metadata descriptor builders

            var metadataDescriptorBuilders = new Func<MetadataAllocationDescriptor>[]
            {
                PoolsFactory.BuildIndexedMetadataDescriptor,
                PoolsFactory.BuildAddressMetadataDescriptor,
                PoolsFactory.BuildVariantMetadataDescriptor
            };

            #endregion

            poolWithAddressBuilder.Initialize();
            
            foreach (var element in settings.Elements)
            {
                #region Address
                
                string fullAddress = element.Name;

                int[] addressHashes = fullAddress.AddressToHashes();

                SetAddressCallback<GameObject> setAddressCallback = PoolsFactory.BuildSetAddressCallback<GameObject>(
                    fullAddress,
                    addressHashes);

                #endregion
                
                poolWithVariantsBuilder.Initialize();

                for (int i = 0; i < element.Variants.Length; i++)
                {
                    #region Variant
                    
                    var currentVariant = element.Variants[i];

                    SetVariantCallback<GameObject> setVariantCallback = PoolsFactory.BuildSetVariantCallback<GameObject>(i);
                    
                    RenameByStringAndIndexCallback renameCallback = PoolsFactory.BuildRenameByStringAndIndexCallback($"{fullAddress} (Variant {i.ToString()})");
                    
                    #endregion
                    
                    #region Allocation callbacks initialization

                    var callbacks = new IAllocationCallback<GameObject>[]
                    {
                        renameCallback,
                        setAddressCallback,
                        setVariantCallback,
                        pushCallback
                    };

                    #endregion
                    
                    #region Value allocation delegate initialization

                    var prefab = currentVariant.Prefab;
                    
                    Func<GameObject> valueAllocationDelegate =
                        () => PoolsFactory.DIResolveOrInstantiateAllocationDelegate(
                            container,
                            prefab);

                    #endregion
                    
                    resizablePoolBuilder.Initialize(
                        valueAllocationDelegate,
                        metadataDescriptorBuilders,
                        currentVariant.initial,
                        currentVariant.additional,
                        callbacks);

                    var resizablePool = resizablePoolBuilder.Build();

                    var gameObjectPool = PoolsFactory.BuildNonAllocGameObjectPool(
                        resizablePool,
                        parentTransform);
                    
                    var prefabInstancePool = PoolsFactory.BuildNonAllocPrefabInstancePool(
                        gameObjectPool,
                        prefab);
                    
                    poolWithVariantsBuilder.AddVariant(
                        i,
                        currentVariant.Chance,
                        prefabInstancePool);
                }

                var variantPool = poolWithVariantsBuilder.Build();
                
                poolWithAddressBuilder.Parse(fullAddress, variantPool);
            }

            var poolWithAddress = poolWithAddressBuilder.Build();

            var poolWithID = PoolsFactory.BuildNonAllocPoolWithID(poolWithAddress, settings.ID);

            pushCallback.Root = poolWithID;
            
            return poolWithID;
        }

        #endregion
    }
}