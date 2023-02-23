using HereticalSolutions.Pools.Arguments;

namespace HereticalSolutions.Pools
{
	public abstract class ANonAllocDecoratorPool<T>
		: INonAllocDecoratedPool<T>
	{
		protected INonAllocDecoratedPool<T> innerPool;

		public ANonAllocDecoratorPool(
			INonAllocDecoratedPool<T> innerPool)
		{
			this.innerPool = innerPool;
		}

		#region Pop

		public virtual IPoolElement<T> Pop(IPoolDecoratorArgument[] args)
		{
			OnBeforePop(args);

			IPoolElement<T> result = innerPool.Pop(args);

			OnAfterPop(result, args);

			return result;
		}

		protected virtual void OnBeforePop(IPoolDecoratorArgument[] args)
		{
		}

		protected virtual void OnAfterPop(
			IPoolElement<T> instance,
			IPoolDecoratorArgument[] args)
		{
		}

		#endregion

		#region Push

		public virtual void Push(
			IPoolElement<T> instance,
			bool dryRun = false)
		{
			OnBeforePush(instance);

			innerPool.Push(
				instance,
				dryRun);

			OnAfterPush(instance);
		}

		protected virtual void OnBeforePush(IPoolElement<T> instance)
		{
		}

		protected virtual void OnAfterPush(IPoolElement<T> instance)
		{
		}

		#endregion
	}
}