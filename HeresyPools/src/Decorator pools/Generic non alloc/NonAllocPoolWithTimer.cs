using System;
using HereticalSolutions.Pools.Arguments;

namespace HereticalSolutions.Pools
{
	public class NonAllocPoolWithTimer<T>
		: ANonAllocDecoratorPool<T>,
		ITimerExpiredNotifier,
		IContainsRootPool<T>
	{
		private INonAllocDecoratedPool<T> innerPool;

		public void SetRootPool(INonAllocDecoratedPool<T> pool)
		{
			innerPool = pool;
		}

		public NonAllocPoolWithTimer(INonAllocDecoratedPool<T> innerPool)
			: base(innerPool)
		{
		}

		public void NotifyTimerExpired(IContainsTimer containsTimer)
		{
			innerPool.Push((IPoolElement<T>)containsTimer);
		}

		protected override void OnAfterPop(
			IPoolElement<T> instance,
			IPoolDecoratorArgument[] args)
		{
			IContainsTimer containsTimer = (IContainsTimer)instance;

			if (containsTimer == null)
				throw new Exception($"[NonAllocPoolWithTimer] INVALID ELEMENT");

			containsTimer.Callback = this;

			if (args.TryGetArgument<TimerArgument>(out var arg))
			{
				containsTimer.Timer.Start(arg.Duration);
			}
			else
				containsTimer.Timer.Start();
		}
	}
}