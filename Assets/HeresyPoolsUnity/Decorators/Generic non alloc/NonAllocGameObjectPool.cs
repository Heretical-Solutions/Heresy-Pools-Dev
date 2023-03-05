using UnityEngine;
using HereticalSolutions.Pools.Arguments;

namespace HereticalSolutions.Pools.Decorators
{
	public class NonAllocGameObjectPool : ANonAllocDecoratorPool<GameObject>
	{
		private readonly Transform poolParentTransform;

		public NonAllocGameObjectPool(INonAllocDecoratedPool<GameObject> innerPool,
			Transform parentTransform)
			: base(innerPool)
		{
			poolParentTransform = parentTransform;
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

			#region Parent transform
			
			if (args.TryGetArgument<ParentTransformArgument>(out var arg1))
			{
				newParentTransform = arg1.Parent;

				worldPositionStays = arg1.WorldPositionStays;
			}
			
			#endregion

			value.transform.SetParent(newParentTransform, worldPositionStays);

			#region World position
			
			if (args.TryGetArgument<WorldPositionArgument>(out var arg2))
			{
				value.transform.position = arg2.Position;
			}
			
			#endregion

			#region World rotation
			
			if (args.TryGetArgument<WorldRotationArgument>(out var arg3))
			{
				value.transform.rotation = arg3.Rotation;
			}
			
			#endregion

			#region Local position
			
			if (args.TryGetArgument<LocalPositionArgument>(out var arg4))
			{
				value.transform.localPosition = arg4.Position;
			}
			
			#endregion

			#region Local rotation
			
			if (args.TryGetArgument<LocalRotationArgument>(out var arg5))
			{
				value.transform.localRotation = arg5.Rotation;
			}
			
			#endregion

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