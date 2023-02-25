using System;

using HereticalSolutions.Timers;

namespace HereticalSolutions.Pools.Factories
{
	public static partial class PoolsFactory
	{
		/*
		#region Pool elements

		public static IPoolElement<T> BuildValueAssignedNotifyingPoolElement<T>(
			Func<T> allocationDelegate,
			INotifiable<T> notifiable)
		{
			ValueAssignedNotifyingPoolElement<T> result = new ValueAssignedNotifyingPoolElement<T>(
				allocationDelegate(),
				notifiable);

			return result;
		}

		public static Func<Func<T>, IPoolElement<T>> BuildValueAssignedNotifyingPoolElementAllocationDelegate<T>(
			INotifiable<T> notifiable)
		{
			return (valueAllocationDelegate) =>
			{
				return BuildValueAssignedNotifyingPoolElement(
					valueAllocationDelegate,
					notifiable);
			};
		}

		public static IPoolElement<T> BuildPoolElementWithAddressAndVariant<T>(
			Func<T> allocationDelegate,
			INotifiable<T> notifiable,
			int[] addressHashes,
			int variant = -1)
		{
			PoolElementWithContainsAddressAndContainsVariant<T> result = new PoolElementWithContainsAddressAndContainsVariant<T>(
				allocationDelegate(),
				notifiable,
				addressHashes,
				variant);

			return result;
		}

		public static Func<Func<T>, IPoolElement<T>> BuildPoolElementWithAddressAndVariantAllocationDelegate<T>(
			INotifiable<T> notifiable,
			int[] addressHashes,
			int variant = -1)
		{
			return (valueAllocationDelegate) =>
			{
				return BuildPoolElementWithAddressAndVariant(
					valueAllocationDelegate,
					notifiable,
					addressHashes,
					variant);
			};
		}

		public static IPoolElement<T> BuildPoolElementWithTimer<T>(
			Func<T> allocationDelegate,
			INotifiable<T> notifiable,
			Timer timer,
			int[] addressHashes,
			int variant = -1)
		{
			PoolElementWithTimer<T> result = new PoolElementWithTimer<T>(
				allocationDelegate(),
				notifiable,
				timer,
				addressHashes,
				variant);

			return result;
		}

		public static Func<Func<T>, IPoolElement<T>> BuildPoolElementWithTimerAllocationDelegate<T>(
			INotifiable<T> notifiable,
			Func<Timer> timerAllocationDelegate,
			int[] addressHashes,
			int variant = -1)
		{
			return (valueAllocationDelegate) =>
			{
				return BuildPoolElementWithTimer(
					valueAllocationDelegate,
					notifiable,
					timerAllocationDelegate(),
					addressHashes,
					variant);
			};
		}

		#endregion
		*/
	}
}