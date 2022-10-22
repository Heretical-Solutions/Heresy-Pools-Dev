namespace HereticalSolutions.Pools
{
	public class PoolElementWithAddressAndVariant<T>
		: ValueAssignedNotifyingPoolElement<T>,
		IAddressContainable,
		IVariantContainable
	{
		public int[] AddressHashes { get; private set; }

		public int Variant { get; private set; }

		public PoolElementWithAddressAndVariant(
			T initialValue,
			IValueAssignedNotifiable<T> notifiable,
			int[] addressHashes,
			int variant = -1)
			: base (
				initialValue,
				notifiable)
		{
			AddressHashes = addressHashes;

			Variant = variant;
		}
	}
}