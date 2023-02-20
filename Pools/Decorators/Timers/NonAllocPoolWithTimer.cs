using System;
using UnityEngine;
using HereticalSolutions.Collections;
using HereticalSolutions.Pools;
using HereticalSolutions.Pools.Arguments;

namespace HereticalSolutions.Pools
{
	public class NonAllocPoolWithTimer
		: ANonAllocDecoratorPool<GameObject>,
		ITimerContainableTimerExpiredNotifiable,
		IPoolProvidable<GameObject>
	{
		private INonAllocDecoratedPool<GameObject> poolWrapper;

		public void SetPool(INonAllocDecoratedPool<GameObject> pool)
		{
			poolWrapper = pool;
		}

		public NonAllocPoolWithTimer(INonAllocDecoratedPool<GameObject> innerPool)
			: base(innerPool)
		{
		}

		public void HandleTimerContainableTimerExpired(ITimerContainable timerContainable)
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

			timerContainable.Callback = this;

			if (args.TryGetArgument<TimerArgument>(out var arg))
			{
				timerContainable.Timer.Start(arg.Duration);
			}
			else
				timerContainable.Timer.Start();
		}
	}
}