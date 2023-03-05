namespace HereticalSolutions.Pools
{
	public static class AddressHelper
	{
		public static int[] AddressToHashes(this string address)
		{
			if (string.IsNullOrEmpty(address))
				return new int[0];
			
			string[] localAddresses = address.Split('/');

			int[] result = new int[localAddresses.Length];

			for (int i = 0; i < result.Length; i++)
				result[i] = localAddresses[i].GetHashCode();

			return result;
		}
	}
}