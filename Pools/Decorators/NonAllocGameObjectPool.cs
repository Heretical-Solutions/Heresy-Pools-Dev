using UnityEngine;
using HereticalSolutions.Collections;
using HereticalSolutions.Pools.Arguments;

namespace HereticalSolutions.Pools
{
	public class NonAllocGameObjectPool : ANonAllocDecoratorPool<GameObject>
	{
		private Transform poolParentTransform;

		public NonAllocGameObjectPool(INonAllocDecoratedPool<GameObject> innerPool,
			Transform parentTransform)
			: base(innerPool)
		{
			this.poolParentTransform = parentTransform;
		}

		protected override void OnAfterPop(
			IPoolElement<GameObject> instance,
			IPoolDecoratorArgument[] args)
		{
			var value = instance.Value;

			if (value == null)
				return;

			Transform newParentTransform = null;

			bool worldPositionStays = true;

			if (args.TryGetArgument<ParentTransformArgument>(out var arg1))
			{
				newParentTransform = arg1.Parent;

				worldPositionStays = arg1.WorldPositionStays;
			}

			value.transform.SetParent(newParentTransform, worldPositionStays);

			if (args.TryGetArgument<WorldPositionArgument>(out var arg2))
			{
				value.transform.position = arg2.Position;
			}

			if (args.TryGetArgument<WorldRotationArgument>(out var arg3))
			{
				value.transform.rotation = arg3.Rotation;
			}

			if (args.TryGetArgument<LocalPositionArgument>(out var arg4))
			{
				value.transform.localPosition = arg4.Position;
			}

			if (args.TryGetArgument<LocalRotationArgument>(out var arg5))
			{
				value.transform.localRotation = arg5.Rotation;
			}

			value.SetActive(true);
		}

		protected override void OnBeforePush(IPoolElement<GameObject> instance)
		{
			var value = instance.Value;

			if (value == null)
				return;

			value.SetActive(false);

			value.transform.SetParent(poolParentTransform);

			value.transform.localPosition = Vector3.zero;

			value.transform.localRotation = Quaternion.identity;
		}
	}
}