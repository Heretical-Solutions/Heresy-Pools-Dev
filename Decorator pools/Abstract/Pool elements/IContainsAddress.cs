namespace HereticalSolutions.Pools
{
	public interface IContainsAddress
	{
		string FullAddress { get; }

		int[] AddressHashes { get; }
	}
}