using System;
using HereticalSolutions.Collections;
using HereticalSolutions.Pools.Arguments;
using HereticalSolutions.Pools.Behaviours;

namespace HereticalSolutions.Pools.Decorators
{
	public class NonAllocDecoratorPool<T> : INonAllocDecoratedPool<T>
	{
		private readonly INonAllocPool<T> innerPool;
		
		private readonly IPushBehaviourHandler<T> pushBehaviourHandler;

		public NonAllocDecoratorPool(INonAllocPool<T> innerPool)
		{
			this.innerPool = innerPool;

			pushBehaviourHandler = new PushToDecoratedPoolBehaviour<T>(this);
		}

		public IPoolElement<T> Pop(IPoolDecoratorArgument[] args)
		{
			#region Append from argument
			
			if (args.TryGetArgument<AppendArgument>(out var arg))
			{
				var appendable = (IAppendable<IPoolElement<T>>)innerPool;

				if (appendable == null)
					throw new Exception("[NonAllocDecoratorPool] POOL IS NOT APPENDABLE");

				var appendee = appendable.Append();
				
				
				//Update element data
				var appendeeElementAsPushable = (IPushable<T>)appendee; 
            
				appendeeElementAsPushable.UpdatePushBehaviour(pushBehaviourHandler);
				

				return appendee;
			}
			
			#endregion

			var result = innerPool.Pop();
			
			
			//Update element data
			var elementAsPushable = (IPushable<T>)result; 
            
			elementAsPushable.UpdatePushBehaviour(pushBehaviourHandler);
			

			#region Top Up from argument

			if (result.Value.Equals(default(T)))
			{
				var topUppable = (ITopUppable<IPoolElement<T>>)innerPool;

				if (topUppable == null)
					throw new Exception("[NonAllocDecoratorPool] POOL ELEMENT IS EMPTY AND POOL IS NOT TOP UPPABLE");
				
				topUppable.TopUp(result);
			}
			
			#endregion

			return result;
		}

		public void Push(
			IPoolElement<T> instance,
			bool decoratorsOnly = false)
		{
			if (!decoratorsOnly)
				innerPool.Push(instance);
		}
	}
}