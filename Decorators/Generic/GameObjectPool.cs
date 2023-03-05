using UnityEngine;
using HereticalSolutions.Pools.Arguments;

namespace HereticalSolutions.Pools.Decorators
{
	public class GameObjectPool : ADecoratorPool<GameObject>
	{
		private readonly Transform poolParentTransform;

		public GameObjectPool(
			IDecoratedPool<GameObject> innerPool,
			Transform parentTransform)
			: base(innerPool)
		{
			poolParentTransform = parentTransform;
		}

		protected override void OnAfterPop(
			GameObject instance,
			IPoolDecoratorArgument[] args)
		{
			Transform newParentTransform = null;

			bool worldPositionStays = true;

			#region Parent transform
			
			if (args.TryGetArgument<ParentTransformArgument>(out var arg1))
			{
				newParentTransform = arg1.Parent;

				worldPositionStays = arg1.WorldPositionStays;
			}
			
			#endregion

			instance.transform.SetParent(newParentTransform, worldPositionStays);

			#region World position
			
			if (args.TryGetArgument<WorldPositionArgument>(out var arg2))
			{
				instance.transform.position = arg2.Position;
			}
			
			#endregion

			#region World rotation
			
			if (args.TryGetArgument<WorldRotationArgument>(out var arg3))
			{
				instance.transform.rotation = arg3.Rotation;
			}
			
			#endregion

			#region Local position

			if (args.TryGetArgument<LocalPositionArgument>(out var arg4))
			{
				instance.transform.localPosition = arg4.Position;
			}
			
			#endregion

			#region Local rotation
			
			if (args.TryGetArgument<LocalRotationArgument>(out var arg5))
			{
				instance.transform.localRotation = arg5.Rotation;
			}
			
			#endregion

			instance.SetActive(true);
		}

		protected override void OnBeforePush(GameObject instance)
		{
			instance.SetActive(false);

			instance.transform.SetParent(poolParentTransform);

			instance.transform.localPosition = Vector3.zero;

			instance.transform.localRotation = Quaternion.identity;
		}
	}
}