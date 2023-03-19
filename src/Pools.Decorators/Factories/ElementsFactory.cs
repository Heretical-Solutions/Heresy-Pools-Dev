using HereticalSolutions.Pools.Allocations;

namespace HereticalSolutions.Pools.Factories
{
	public static partial class PoolsFactory
	{
		#region Metadata

		public static AddressMetadata BuildAddressMetadata()
		{
			return new AddressMetadata();
		}

		public static MetadataAllocationDescriptor BuildAddressMetadataDescriptor()
		{
			return new MetadataAllocationDescriptor
			{
				BindingType = typeof(IContainsAddress),
				ConcreteType = typeof(AddressMetadata)
			};
		}
		
		public static VariantMetadata BuildVariantMetadata()
		{
			return new VariantMetadata();
		}

		public static MetadataAllocationDescriptor BuildVariantMetadataDescriptor()
		{
			return new MetadataAllocationDescriptor
			{
				BindingType = typeof(IContainsVariant),
				ConcreteType = typeof(VariantMetadata)
			};
		}

		#endregion
	}
}