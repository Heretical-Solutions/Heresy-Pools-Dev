using System;
using System.Text;

namespace HereticalSolutions.Pools
{
	public static class AddressHelper
	{
		public static string[] SplitAddressBySeparator(this string address)
		{
			if (string.IsNullOrEmpty(address))
				return new string[0];
			
			string[] localAddresses = address.Split('/');

			return localAddresses;
		}

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

		public static int[] PartialAddressHashes(this int[] addressHashes, int depth)
		{	
			int effectiveDepth = Math.Min(addressHashes.Length, depth);

			int[] result = new int[effectiveDepth];
			
			for (int i = 0; i < result.Length; i++)
				result[i] = addressHashes[i];

			return result;
		}

		public static string AddressAtDepth(this string[] addressParts, int depth)
		{
			return addressParts[depth];
		}

		public static string PartialAddress(this string[] addressParts, int depth)
		{
			int effectiveDepth = Math.Min(addressParts.Length, depth);
			
			StringBuilder stringBuilder = new StringBuilder();

			for (int i = 0; i < effectiveDepth; i++)
			{
				stringBuilder.Append(addressParts[i]);

				if (i + 1 < effectiveDepth)
					stringBuilder.Append('/');
			}

			return stringBuilder.ToString();
		}
	}
}