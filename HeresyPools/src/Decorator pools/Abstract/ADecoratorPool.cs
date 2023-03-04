using HereticalSolutions.Pools.Arguments;

namespace HereticalSolutions.Pools
{
	public abstract class ADecoratorPool<T>
		: IDecoratedPool<T>
	{
		protected IDecoratedPool<T> innerPool;

		public ADecoratorPool(
			IDecoratedPool<T> innerPool)
		{
			this.innerPool = innerPool;
		}

		#region Pop

		public virtual T Pop(IPoolDecoratorArgument[] args)
		{
			OnBeforePop(args);

			T result = innerPool.Pop(args);

			OnAfterPop(result, args);

			return result;
		}

		protected virtual void OnBeforePop(IPoolDecoratorArgument[] args)
		{
		}

		protected virtual void OnAfterPop(
			T instance,
			IPoolDecoratorArgument[] args)
		{
		}

		#endregion

		#region Push

		public virtual void Push(
			T instance,
			bool decoratorsOnly = false)
		{
			OnBeforePush(instance);

			innerPool.Push(
				instance,
				decoratorsOnly);

			OnAfterPush(instance);
		}

		protected virtual void OnBeforePush(T instance)
		{
		}

		protected virtual void OnAfterPush(T instance)
		{
		}

		#endregion
	}
}