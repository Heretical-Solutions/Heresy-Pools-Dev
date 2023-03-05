using UnityEngine;

namespace HereticalSolutions.Pools.Factories
{
	public static partial class PoolsFactory
	{
		#region Default allocation delegates

		public static GameObject InstantiateAllocationDelegate(GameObject prefab)
		{
			return GameObject.Instantiate(prefab) as GameObject;
		}

		#endregion
	}
}