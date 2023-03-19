using HereticalSolutions.Pools.Arguments;
using HereticalSolutions.Pools.Behaviours;

namespace HereticalSolutions.Pools
{
	public abstract class ANonAllocDecoratorPool<T>
		: INonAllocDecoratedPool<T>
	{
		protected INonAllocDecoratedPool<T> innerPool;
		
		private readonly IPushBehaviourHandler<T> pushBehaviourHandler;

		public ANonAllocDecoratorPool(
			INonAllocDecoratedPool<T> innerPool)
		{
			this.innerPool = innerPool;

			pushBehaviourHandler = new PushToDecoratedPoolBehaviour<T>(this);
		}

		#region Pop

		public virtual IPoolElement<T> Pop(IPoolDecoratorArgument[] args)
		{
			OnBeforePop(args);

			IPoolElement<T> result = innerPool.Pop(args);

			#region Update push behaviour
			
			var elementAsPushable = (IPushable<T>)result; 
            
			elementAsPushable.UpdatePushBehaviour(pushBehaviourHandler);
			
			#endregion
			
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
			bool decoratorsOnly = false)
		{
			OnBeforePush(instance);

			innerPool.Push(
				instance,
				decoratorsOnly);

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