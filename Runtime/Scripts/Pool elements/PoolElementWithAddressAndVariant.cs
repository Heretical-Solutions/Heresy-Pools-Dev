namespace HereticalSolutions.Pools
{
	public class PoolElementWithAddressAndVariant<T>
		: ValueAssignedNotifyingPoolElement<T>,
		IAddressContainable,
		IVariantContainable
	{
		public string Address { get; private set; }

		public int Variant { get; private set; }

		public PoolElementWithAddressAndVariant(
			T initialValue,
			IValueAssignedNotifiable<T> notifiable,
			string address,
			int variant = -1)
			: base (
				initialValue,
				notifiable)
		{
			Address = address;

			Variant = variant;
		}
	}
}