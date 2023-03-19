<img src="docs/Images/logo.png?raw=true" alt="Heresy Pools" width="500px" height="250px"/>

Heresy Pools
===
Versatile, scalable and memory efficient memory pool solution for your projects

## Table Of Contents

<details>
<summary>Details</summary>

- [About](#about)
  - [Introduction](#introduction)
  - [Why Heretical Pools?](#why-heretical-pools)
  - [Installation](#installation)
- [Usage](#usage)
- [Documentation](#documentation)

</details>

## About

### Introduction

Heresy Pools is a 

### Why Heretical Pools?

* Easy to use
* Engine agnostic (tested on Unity but any C# based engine can make use of the solution)
* Scalable (provides API to extend pools with new behavior and metadata)
* Memory efficient (provides API for alloc free popping and pushing elements)
* Provides better control over allocations and resizing behaviour

### Installation

You can install Heresy Pools using any of the following methods

1.  __From [UPM Package](https://github.com/Heretical-Solutions/Heresy-Pools/tree/UPM)__
    Find the `manifest.json` file in the `Packages` directory in your project and edit it as follows:
    ```
    {

      "dependencies": {

        "com.heretical-solutions.heresy-pools": "https://github.com/Heretical-Solutions/HeresyPools.git#UPM",

        ...

      },

    }

    ```
	Alternatively, open the Package Manager, press the "+" button, select "Add package from git URL" and paste in 'https://github.com/Heretical-Solutions/HeresyPools.git#UPM'

2.  __By building a DLL from [Dev branch](https://github.com/Heretical-Solutions/Heresy-Pools/tree/Dev)__
    * Clone the project with git
	* Switch to Dev branch
	* Open the solution file with an IDE of your preference
	* Open the terminal
	* Type in 'dotnet build' and hit enter (requires NET Framewok installed)
	* The dll will be located in 'bin' folder

3.  __By copying source code from [Source branch](https://github.com/Heretical-Solutions/Heresy-Pools/tree/Source)__


## Usage

### Simple object pool

```csharp
//Initial allocation descriptor. Tells the factory how many pool elements should be allocated during its creation
//In this case, the pool will be allocated with 100 elements
var initial = new AllocationCommandDescriptor
{
	Rule = EAllocationAmountRule.ADD_PREDEFINED_AMOUNT, //Allocation rule enum
	Amount = 100 //Allocation amount
}

//Additional allocation descriptor. Tells the factory how many pool elements should be added to the pool once it runs out of elements
//In this case, every time pool is asked to Pop and has all its elements already popped it will allocate the same amount of elements that it already has
var additional = new AllocationCommandDescriptor
{
	Rule = EAllocationAmountRule.DOUBLE_AMOUNT, //Allocation rule enum
	Amount = 0 //Optional for the DOUBLE_AMOUNT rule
}

//Build a generic pool based on a stack
var pool = PoolsFactory.BuildStackPool<T>(
	initial,
	additional);

//Pop	
var element = pool.Pop();

//Push
pool.Push(element);

```

### Resizable non-allocating object pool

```csharp
//Value allocation delegate. Tells the pool how to allocate a value for every new pool element instance
//A set of ready-to-use delegates are included in the pool factory
Func<T> valueAllocationDelegate = PoolsFactory.NullAllocationDelegate<T>;

//A collection of metadata descriptor builders. Metadata is a collection of additional information included into every pool element upon allocation
//In this case, the only information included is element's own index in the pool's packed array. This one must be included into any decorated pool element
var metadataDescriptorBuilders = new Func<MetadataAllocationDescriptor>[]
{
	PoolsFactory.BuildIndexedMetadataDescriptor()
}

//Initial allocation descriptor. Tells the factory how many pool elements should be allocated during its creation
//In this case, the pool will be allocated with 100 elements
var initial = new AllocationCommandDescriptor
{
	Rule = EAllocationAmountRule.ADD_PREDEFINED_AMOUNT, //Allocation rule enum
	Amount = 100 //Allocation amount
}

//Additional allocation descriptor. Tells the factory how many pool elements should be added to the pool once it runs out of elements
//In this case, every time pool is asked to Pop and has all its elements already popped it will allocate the same amount of elements that it already has
var additional = new AllocationCommandDescriptor
{
	Rule = EAllocationAmountRule.DOUBLE_AMOUNT, //Allocation rule enum
	Amount = 0 //Optional for the DOUBLE_AMOUNT rule
}

//Allocation callbacks array. Allocation callbacks are called once for each pool element upon its allocation, modifying its metadata and performing user specified logic
//Right now we don't need any of it so the array is empty on purpose
var callbacks = new IAllocationCallback<T>[0];

//Create a pool builder
ResizablePoolBuilder<T> resizablePoolBuilder = new ResizablePoolBuilder<T>();

//Initialize a pool builder with values
resizablePoolBuilder.Initialize(
	valueAllocationDelegate,
	metadataDescriptorBuilders,
	initialAllocation,
	additionalAllocation,
	callbacks);

//Build a pool
var pool = resizablePoolBuilder.Build();

//Pop an element. Null means an empty arguments array
//Returns IPoolElement - a 'smart reference' to the actual pooled value
var element = pool.Pop(null);

//Value access
T value = element.Value;

//Push
pool.Push(element);

//IPoolElement contain a reference to the pool it's in so that we don't try to push an element from one pool into another by accident
//This also allows us to call Push on the element itself - it will know wnere to be pushed
element.Push();

```

### Resizable non-allocating game object pool (Unity)

Requires [Unity Source](https://github.com/Heretical-Solutions/Heresy-Pools/tree/Source_Unity) branch contents

```csharp
//Allocation command descriptors have [System.Serializable] attribute
[SerializeField]
private AllocationCommandDescriptor initial;

[SerializeField]
private AllocationCommandDescriptor additional;

//Cached 'world position' argument. Tells the pool where to place a game object when it's popped
private WorldPositionArgument worldPositionArgument;

//Cached arguments array, passed to the pool during the pop call
var argumentsCache = new ArgumentBuilder()
	.Add<WorldPositionArgument>(out worldPositionArgument)
	.Build();

//Build simple game object pool from template factory
var gameObjectPool = PoolsFactory.BuildSimpleGameObjectPool(
	id, //Pool ID, string
	prefab, //Gameobject prefab
	poolParent, //Transform that will parent unused pool element instances, optional
	initial, //Initial allocation descriptior
	additional); //Additional allocation descriptor

//Set desired position to the argument
worldPositionArgument.Position = new Vector3(1f, 2f, -3f);

//Pop
var element = gameObjectPool.Pop(argumentsCache);

//Value access
GameObject value = element.Value;

//Push
pool.Push(element);

//IPoolElement contain a reference to the pool it's in so that we don't try to push an element from one pool into another by accident
//This also allows us to call Push on the element itself - it will know wnere to be pushed
element.Push();
```

### Adding my own decorator

### Adding my own metadata

### Samples

The [Unity Dev branch](https://github.com/Heretical-Solutions/Heresy-Pools/tree/Dev_Unity) contains a sample project with all dependencies in Assets folder. The sample scene is located in `Scenes/SampleScene` directory

## Documentation