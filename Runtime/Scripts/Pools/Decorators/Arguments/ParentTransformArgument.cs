using UnityEngine;

namespace HereticalSolutions.Pools.Arguments
{
	public class ParentTransformArgument : IPoolDecoratorArgument
	{
		public Transform Parent;

		public bool WorldPositionStays = true;
	}
}