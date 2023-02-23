using System;
using HereticalSolutions.Collections;
using HereticalSolutions.Pools.Arguments;

namespace HereticalSolutions.Pools
{
	public class NonAllocPoolWithTimer<T>
		: ANonAllocDecoratorPool<T>,
		ITimerExpiredNotifier,
		IPoolProvidable<T>
	{
		private INonAllocDecoratedPool<T> innerPool;

		public void SetPool(INonAllocDecoratedPool<T> pool)
		{
			innerPool = pool;
		}

		public NonAllocPoolWithTimer(INonAllocDecoratedPool<T> innerPool)
			: base(innerPool)
		{
		}

		public void NotifyTimerExpired(ITimerContainable timerContainable)
		{
			innerPool.Push((IPoolElement<T>)timerContainable);
		}

		protected override void OnAfterPop(
			IPoolElement<T> instance,
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