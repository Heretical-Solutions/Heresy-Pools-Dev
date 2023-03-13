namespace HereticalSolutions.Pools
{
    public class AddressMetadata : IContainsAddress
    {
        public string FullAddress { get; set; }
        public int[] AddressHashes { get; set; }

        public AddressMetadata()
        {
            FullAddress = string.Empty;

            AddressHashes = new int[0];
        }
    }
}