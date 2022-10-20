using HereticalSolutions.Collections;
using HereticalSolutions.Collections.Managed;

namespace HereticalSolutions.Pools
{
	public class ValueAssignedNotifyingPoolElement<T>
		: IPoolElement<T>,
		  IIndexed
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

				if (Index == -1)
					notifiable.Notify(this);
			}
		}

		public ValueAssignedNotifyingPoolElement(
			T initialValue,
			IValueAssignedNotifiable<T> notifiable)
		{
			this.notifiable = notifiable;

			Index = -1;

			Value = initialValue;
		}
	}
}