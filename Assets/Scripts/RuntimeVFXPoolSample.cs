using UnityEngine;

using HereticalSolutions.Pools;
using HereticalSolutions.Pools.Factories;

using HereticalSolutions.Collections;
using HereticalSolutions.Pools.Arguments;

using HereticalSolutions.Collections.Allocations;

public class RuntimeVFXPoolSample : MonoBehaviour
{
	[Header("Settings")]

	[SerializeField]
	private PoolSettings poolSettings;
   
	[SerializeField]
	private Transform poolParent;

    [Space]

    [Header("Debug")]

    [SerializeField]
    private bool popPerformed;
	
    [SerializeField]
    private bool pushedPerformed;
    
    
    private INonAllocDecoratedPool<GameObject> gameObjectPool;

    
    private INonAllocPool<IPoolElement<GameObject>> poppedElements;

    private IIndexable<IPoolElement<IPoolElement<GameObject>>> poppedElementsAsIndexable;


    private AddressArgument addressArgument;
    
    private WorldPositionArgument worldPositionArgument;

    private IPoolDecoratorArgument[] argumentsCache;

    void Start()
    {
	    gameObjectPool = PoolsFactory.BuildPool(
	        null,
	        poolSettings,
	        poolParent);

        poppedElements = PoolsFactory.BuildPackedArrayPool<IPoolElement<GameObject>>(
	        PoolsFactory.BuildPoolElementAllocationCommand<IPoolElement<GameObject>>(
		        new AllocationCommandDescriptor
		        {
			        Rule = EAllocationAmountRule.ADD_PREDEFINED_AMOUNT,
			        Amount = 100
		        },
		        PoolsFactory.NullAllocationDelegate<IPoolElement<GameObject>>,
		        new[]
		        {
			        PoolsFactory.BuildIndexedMetadataDescriptor()
		        }));

        poppedElementsAsIndexable = (IIndexable<IPoolElement<IPoolElement<GameObject>>>)poppedElements;
        
        argumentsCache = new ArgumentBuilder()
            .Add<WorldPositionArgument>(out worldPositionArgument)
            .Add<AddressArgument>(out addressArgument)
            .Build();
    }

    // Update is called once per frame
    void Update()
    {
	    bool doSomething = UnityEngine.Random.Range(0f, 1f) < 0.1f;

	    if (!doSomething)
	    {
		    //Uncomment for runtime accuracy
	        
		    //popPerformed = false;

		    //pushedPerformed = false;
	        
		    return;
	    }

	    bool push = UnityEngine.Random.Range(0f, 1f) < 0.5f;

	    popPerformed = !push;
        
	    pushedPerformed = push;
		

	    if (push)
	    {
		    PushRandomElement();
	    }
	    else
	    {
		    PopRandomElement();
	    }
    }

    private void PushRandomElement()
    {
	    if (poppedElementsAsIndexable.Count == 0)
		    return;

	    var randomIndex = UnityEngine.Random.Range(0, poppedElementsAsIndexable.Count);

	    var activeElement = poppedElementsAsIndexable[randomIndex];

	    //Both options should work the same way
	    //nonAllocPool.Push(activeElement.Value);
	    activeElement.Value.Push();

	    poppedElements.Push(activeElement);
    }

    private void PopRandomElement()
    {
	    worldPositionArgument.Position = new Vector3(
		    UnityEngine.Random.Range(-5f, 5f),
		    UnityEngine.Random.Range(-5f, 5f),
		    UnityEngine.Random.Range(-5f, 5f));

	    var address = poolSettings.Elements[UnityEngine.Random.Range(0, poolSettings.Elements.Length)].Name;

	    addressArgument.AddressHashes = address.AddressToHashes();
	    
	    var value = gameObjectPool.Pop(argumentsCache);

	    var activeElement = poppedElements.Pop();

	    activeElement.Value = value;
    }
}
