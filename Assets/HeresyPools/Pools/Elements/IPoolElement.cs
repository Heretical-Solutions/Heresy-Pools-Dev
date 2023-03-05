namespace HereticalSolutions.Pools
{
	public interface IPoolElement<T>
	{
		T Value { get; set; }

		EPoolElementStatus Status { get; }

		IMetadataCollection Metadata { get; }

		void Push();
	}
}