using UnityEngine;
using System;
using HereticalSolutions.Messaging;
using HereticalSolutions.Timers;

namespace HereticalSolutions.Pools
{
	public class PoolElementWithTimer<T>
		: PoolElementWithAddressAndVariant<T>,
		ITimerContainable
	{
		public Timer Timer { get; private set;}

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
		}
	}
}