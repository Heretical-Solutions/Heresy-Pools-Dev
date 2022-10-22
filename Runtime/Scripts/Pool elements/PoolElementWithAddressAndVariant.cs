using HereticalSolutions.Collections;
using HereticalSolutions.Collections.Managed;

namespace HereticalSolutions.Pools
{
	public class PoolElementWithAddressAndVariant<T>
		: IPoolElement<T>,
		  IIndexed,
		  IAddressContainable,
		  IVariantContainable
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

		public PoolElementWithAddressAndVariant(
			T initialValue,
			IValueAssignedNotifiable<T> notifiable,
			int[] addressHashes,
			int variant = -1)
		{
			Index = -1;

			this.notifiable = notifiable;

			AddressHashes = addressHashes;

			Variant = variant;

			//For obvious reasons, this call should be done last
			//And that's why I've given up on inheriting all succeeding element types from ValueAssignedNotifyingPoolElement
			Value = initialValue;
		}
	}
}