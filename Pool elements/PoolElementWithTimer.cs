using HereticalSolutions.Collections;
using HereticalSolutions.Collections.Managed;
using HereticalSolutions.Timers;


namespace HereticalSolutions.Pools
{
	public class PoolElementWithTimer<T>
		: IPoolElement<T>,
		  IIndexed,
		  IAddressContainable,
		  IVariantContainable,
		  ITimerContainable,
		  ITimerExpiredNotifiable
	{
		public int Index { get; set; }

		protected T innerValue = default(T);

		protected IValueAssignedNotifiable<T> notifiable;

		public T Value
		{
			get => innerValue;
			set
			{
				innerValue = value;

				notifiable?.Notify(this);
			}
		}

		public int[] AddressHashes { get; private set; }

		public int Variant { get; private set; }

		public Timer Timer { get; private set; }

		public ITimerContainableTimerExpiredNotifiable Callback { get; set; }

		public PoolElementWithTimer(
			T initialValue,
			IValueAssignedNotifiable<T> notifiable,
			Timer timer,
			int[] addressHashes,
			int variant = -1)
		{
			Index = -1;

			this.notifiable = notifiable;

			AddressHashes = addressHashes;

			Variant = variant;

			Timer = timer;

			Timer.Callback = this;

			//For obvious reasons, this call should be done last
			//And that's why I've given up on inheriting all succeeding element types from ValueAssignedNotifyingPoolElement
			Value = initialValue;
		}

		public void HandleTimerExpired(Timer timer)
		{
			Callback?.HandleTimerContainableTimerExpired(this);
		}
	}
}