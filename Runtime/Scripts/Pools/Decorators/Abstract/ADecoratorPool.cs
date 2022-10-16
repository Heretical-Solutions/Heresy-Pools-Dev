using HereticalSolutions.Collections;

namespace HereticalSolutions.Pools
{
	public abstract class ADecoratorPool<T>
		: IPool<T>
	{
		protected IPool<T> innerPool;

		public ADecoratorPool(
			IPool<T> innerPool)
		{
			this.innerPool = innerPool;
		}

		#region Pop

		public T Pop()
		{
			OnBeforePop();

			T result = innerPool.Pop();

			OnAfterPop(result);

			return result;
		}

		protected virtual void OnBeforePop()
		{
		}

		protected virtual void OnAfterPop(T instance)
		{
		}

		#endregion

		#region Push

		public void Push(T instance)
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