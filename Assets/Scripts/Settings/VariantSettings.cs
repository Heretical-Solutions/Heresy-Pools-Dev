using System;

using HereticalSolutions.Collections.Allocations;

using UnityEngine;

[Serializable]
public class VariantSettings
{
    public float Chance;

    public GameObject Prefab;
    
    public float Duration;

    public AllocationCommandDescriptor initial;

    public AllocationCommandDescriptor additional;
}