using HereticalSolutions.Collections;

namespace HereticalSolutions.Pools
{
	public abstract class ANonAllocDecoratorPool<T>
		: INonAllocPool<T>
	{
		protected INonAllocPool<T> innerPool;

		public ANonAllocDecoratorPool(
			INonAllocPool<T> innerPool)
		{
			this.innerPool = innerPool;
		}

		#region Pop

		public IPoolElement<T> Pop()
		{
			OnBeforePop();

			IPoolElement<T> result = innerPool.Pop();

			OnAfterPop(result);

			return result;
		}

		protected virtual void OnBeforePop()
		{
		}

		protected virtual void OnAfterPop(IPoolElement<T> instance)
		{
		}

		#endregion

		#region Push

		public void Push(IPoolElement<T> instance)
		{
			OnBeforePush(instance);

			innerPool.Push(instance);

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