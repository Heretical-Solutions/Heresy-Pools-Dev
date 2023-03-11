using UnityEngine;

using HereticalSolutions.Pools;
using HereticalSolutions.Pools.Factories;

using HereticalSolutions.Collections;
using HereticalSolutions.Collections.Managed;
using HereticalSolutions.Collections.Factories;

using HereticalSolutions.Allocations;

using HereticalSolutions.Pools.Arguments;

using System.Collections.Generic;
using HereticalSolutions.Collections.Allocations;
using HereticalSolutions.Pools.AllocationProcessors;
using HereticalSolutions.Pools.Decorators;
using HereticalSolutions.Pools.GenericNonAlloc;
using HereticalSolutions.Repositories;
using HereticalSolutions.Repositories.Factories;

public class RuntimeGameObjectPoolWithVariantsSample : MonoBehaviour
{
    [Header("Settings")]

	[SerializeField]
	private string id;

    [SerializeField]
    private VariantDescriptor[] variantDescriptors;

	[SerializeField]
	private Transform poolParent;

	[Space]

	[Header("Initial allocation")]

	[SerializeField]
	private AllocationCommandDescriptor initial;

	[Space]

	[Header("Additional allocation")]

    [SerializeField]
    private AllocationCommandDescriptor additional;

    [Space]

    [Header("Debug")]

    [SerializeField]
    private bool popPerformed;
	
    [SerializeField]
    private bool pushedPerformed;
    
    
    private INonAllocDecoratedPool<GameObject> decoratedGameObjectPool;

    
    private INonAllocPool<IPoolElement<GameObject>> poppedElements;

    
    private WorldPositionArgument worldPositionArgument;

    private IPoolDecoratorArgument[] argumentsCache;

    void Start()
    {
        decoratedGameObjectPool = BuildNonAllocPool(
			variantDescriptors,
            poolParent,
            initial,
            additional);

        poppedElements = CollectionFactory.BuildIndexedPackedArray<IPoolElement<GameObject>>(
			CollectionFactory.BuildPoolElementAllocationCommand<IPoolElement<GameObject>>(
                new AllocationCommandDescriptor
                {
                    Rule = EAllocationAmountRule.ADD_PREDEFINED_AMOUNT,
                    Amount = 100
                },
				() => { return null; },
				CollectionFactory.BuildIndexedContainer));

        argumentsCache = new ArgumentBuilder()
            .Add<WorldPositionArgument>(out worldPositionArgument)
            .Build();
    }

    [System.Serializable]
    public struct VariantDescriptor
    {
        public float Chance;

        public GameObject Prefab;
    }

	private static INonAllocDecoratedPool<GameObject> BuildNonAllocPool(
		VariantDescriptor[] variants,
		Transform poolParent,
		AllocationCommandDescriptor initial,
		AllocationCommandDescriptor additional)
	{
        IRepository<int, VariantContainer<GameObject>> repository = RepositoriesFactory.BuildDictionaryRepository<int, VariantContainer<GameObject>>();

        //List<CompositeGameObjectAllocationProcessor> processors = new List<CompositeGameObjectAllocationProcessor>();

        for (int i = 0; i < variants.Length; i++)
        {
            var currentVariant = variants[i];

            repository.Add(
                i,
                new VariantContainer<GameObject>
                {
                    Chance = currentVariant.Chance,

                    Pool = BuildVariantPool(
                        i,
                        currentVariant.Prefab,
                        poolParent,
                        initial,
                        additional,
                        out var processor)
                });

            processors.Add(processor);
        }

		//var repository = new DictionaryRepository<int, VariantContainer<GameObject>>(repository);

		var builder = new NonAllocDecoratedPoolBuilder<GameObject>();

		builder
			.Add(new PoolWithVariants<GameObject>(repository));

        foreach (var processor in processors)
			processor.SetPool(builder.CurrentWrapper);

		return builder.CurrentWrapper;
	}

    private static INonAllocDecoratedPool<GameObject> BuildVariantPool(
		int variant,
        GameObject prefab,
		Transform poolParent,
		AllocationCommandDescriptor initial,
		AllocationCommandDescriptor additional,
        out CompositeGameObjectAllocationProcessor processor)
    {
		NewGameObjectsPusher pusher = new NewGameObjectsPusher();

		PoolElementBehaviourInitializer initializer = new PoolElementBehaviourInitializer();

        NameByStringAndIndex namer = new NameByStringAndIndex(prefab.name);

		processor = new CompositeGameObjectAllocationProcessor(
			new Stack<IPoolElement<GameObject>>(),
			new IAllocationProcessor[]
			{
                initializer,
                pusher,
				namer
			});

        INonAllocPool<GameObject> packedArrayPool = PoolFactory.BuildGameObjectPool(
            new BuildNonAllocGameObjectPoolCommand
            {
                CollectionType = typeof(PackedArrayPool<GameObject>),
                Prefab = prefab,
                InitialAllocation = initial,
                AdditionalAllocation = additional,
                ContainerAllocationDelegate = PoolFactory.BuildPoolElementWithAddressAndVariantAllocationDelegate<GameObject>(
                    processor,
                    new int[0],
                    variant)
            });

		var builder = new NonAllocDecoratedPoolBuilder<GameObject>();

		builder
			.Add(new NonAllocWrapperPool<GameObject>(packedArrayPool))
			.Add(new NonAllocGameObjectPool(builder.CurrentWrapper, poolParent));

		//processor.SetWrapper(builder.CurrentWrapper);

        return builder.CurrentWrapper;
    }

    // Update is called once per frame
    void Update()
    {
        bool doSomething = UnityEngine.Random.Range(0f, 1f) < 0.1f;

        if (!doSomething)
            return;

        bool push = UnityEngine.Random.Range(0f, 1f) < 0.5f;

        if (push)
        {
            if (poppedElements.Count == 0)
                return;

            var randomIndex = UnityEngine.Random.Range(0, poppedElements.Count);

            var activeElement = poppedElements[randomIndex];

            decoratedGameObjectPool.Push(activeElement.Value);

            poppedElements.Push(activeElement);
        }
        else
        {
            worldPositionArgument.Position = new Vector3(
                UnityEngine.Random.Range(-5f, 5f),
				UnityEngine.Random.Range(-5f, 5f),
				UnityEngine.Random.Range(-5f, 5f));

            var value = decoratedGameObjectPool.Pop(argumentsCache);

            var activeElement = poppedElements.Pop();

            activeElement.Value = value;
        }

        //Debug.Break();
    }
}
