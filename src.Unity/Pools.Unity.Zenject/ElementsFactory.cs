using Zenject;

using UnityEngine;

namespace HereticalSolutions.Pools.Factories
{
    public static partial class PoolsFactory
    {
        #region Default allocation delegates

        public static GameObject DIResolveAllocationDelegate(
            DiContainer container,
            GameObject prefab)
        {
            return container.InstantiatePrefab(prefab);
        }
		
        public static GameObject DIResolveOrInstantiateAllocationDelegate(
            DiContainer container,
            GameObject prefab)
        {
            return container != null
                ? container.InstantiatePrefab(prefab)
                : GameObject.Instantiate(prefab) as GameObject;
        }

        #endregion
    }
}