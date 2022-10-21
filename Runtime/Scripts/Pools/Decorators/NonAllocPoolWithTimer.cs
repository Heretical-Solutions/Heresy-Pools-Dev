using System;
using UnityEngine;
using HereticalSolutions.Collections;
using HereticalSolutions.Pools.Arguments;

namespace HereticalSolutions.Pools
{
	public class NonAllocPoolWithTimer : ANonAllocDecoratorPool<GameObject>
	{
		private INonAllocDecoratedPool<GameObject> poolWrapper;

		public void SetWrapper(INonAllocDecoratedPool<GameObject> poolWrapper)
		{
			this.poolWrapper = poolWrapper;
		}

		public NonAllocPoolWithTimer(INonAllocDecoratedPool<GameObject> innerPool)
			: base(innerPool)
		{
		}

		private void TimerExpired(ITimerContainable timerContainable)
		{
			poolWrapper.Push((IPoolElement<GameObject>)timerContainable);
		}

		protected override void OnAfterPop(
			IPoolElement<GameObject> instance,
			IPoolDecoratorArgument[] args)
		{
			ITimerContainable timerContainable = (ITimerContainable)instance;

			if (timerContainable == null)
				throw new Exception($"[NonAllocPoolWithTimer] INVALID ELEMENT");

			if (timerContainable.Timer.Callback == null)
				timerContainable.Timer.Callback = () => 
				{
					TimerExpired(timerContainable);
				};

			if (args.TryGetArgument<TimerArgument>(out var arg))
			{
				timerContainable.Timer.Start(arg.Duration);
			}
			else
				timerContainable.Timer.Start();
		}
	}
}