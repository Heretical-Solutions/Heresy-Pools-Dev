using UnityEngine;
using System;
using HereticalSolutions.Messaging;
using HereticalSolutions.Timers;
using HereticalSolutions.Pools;

namespace HereticalSolutions.Pools
{
	public class PoolElementWithTimer<T>
		: PoolElementWithAddressAndVariant<T>,
		ITimerContainable,
		ITimerExpiredNotifiable
	{
		public Timer Timer { get; private set; }

		public ITimerContainableTimerExpiredNotifiable Callback { get; set; }

		public PoolElementWithTimer(
			T initialValue,
			IValueAssignedNotifiable<T> notifiable,
			Timer timer,
			string address,
			int variant)
			: base(
				initialValue,
				notifiable,
				address,
				variant)
		{
			Timer = timer;

			Timer.Callback = this;
		}

		public void HandleTimerExpired(Timer timer)
		{
			Callback.HandleTimerContainableTimerExpired(this);
		}
	}
}