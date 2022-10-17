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

		public virtual T Pop(params IPoolDecoratorArgument[] args)
		{
			OnBeforePop(args);

			T result = innerPool.Pop(args);

			OnAfterPop(result, args);

			return result;
		}

		protected virtual void OnBeforePop(params IPoolDecoratorArgument[] args)
		{
		}

		protected virtual void OnAfterPop(
			T instance,
			params IPoolDecoratorArgument[] args)
		{
		}

		#endregion

		#region Push

		public virtual void Push(T instance)
		{
			OnBeforePush(instance);

			innerPool.Push(instance);

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