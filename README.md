<img src="docs/Images/logo.png?raw=true" alt="Heresy Pools" width="1000px" height="250px"/>

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
* Engine agnostic (tested on Unity but any C#-based engine can make use of the solution)
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

### Simple game object pool (Unity)

### Game object pool with variants

### Adding my own decorator

### Adding my own metadata

### Samples

## Documentation