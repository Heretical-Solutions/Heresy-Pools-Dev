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

				notifiable?.Notify(this);
			}
		}

		public ValueAssignedNotifyingPoolElement(
			T initialValue,
			IValueAssignedNotifiable<T> notifiable)
		{
			Index = -1;

			this.notifiable = notifiable;

			//For obvious reasons, this call should be done last
			Value = initialValue;
		}
	}
}