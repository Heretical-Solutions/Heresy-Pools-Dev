using UnityEngine;
using HereticalSolutions.Pools.Arguments;

namespace HereticalSolutions.Pools
{
	public class GameObjectPool : ADecoratorPool<GameObject>
	{
		private Transform poolParentTransform;

		public GameObjectPool(
			IDecoratedPool<GameObject> innerPool,
			Transform parentTransform)
			: base(innerPool)
		{
			this.poolParentTransform = parentTransform;
		}

		protected override void OnAfterPop(
			GameObject instance,
			IPoolDecoratorArgument[] args)
		{
			Transform newParentTransform = null;

			bool worldPositionStays = true;

			if (args.TryGetArgument<ParentTransformArgument>(out var arg1))
			{
				newParentTransform = arg1.Parent;

				worldPositionStays = arg1.WorldPositionStays;
			}

			instance.transform.SetParent(newParentTransform, worldPositionStays);

			if (args.TryGetArgument<WorldPositionArgument>(out var arg2))
			{
				instance.transform.position = arg2.Position;
			}

			if (args.TryGetArgument<WorldRotationArgument>(out var arg3))
			{
				instance.transform.rotation = arg3.Rotation;
			}

			if (args.TryGetArgument<LocalPositionArgument>(out var arg4))
			{
				instance.transform.localPosition = arg4.Position;
			}

			if (args.TryGetArgument<LocalRotationArgument>(out var arg5))
			{
				instance.transform.localRotation = arg5.Rotation;
			}

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