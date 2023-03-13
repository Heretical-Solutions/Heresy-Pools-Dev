using System.Collections.Generic;

namespace HereticalSolutions.Repositories
{
	/// <summary>
	/// Repository interface
	/// </summary>
	/// <typeparam name="TKey">Key data type</typeparam>
	/// <typeparam name="TValue">Value data type</typeparam>
	public interface IReadOnlyRepository<TKey, TValue>
	{
		/// <summary>
		/// Does the repository have the data by the given key?
		/// </summary>
		/// <param name="key">Key</param>
		/// <returns>Does it or not</returns>
		bool Has(TKey key);

		/// <summary>
		/// Retrieve the data by the given key
		/// </summary>
		/// <param name="key">Key</param>
		/// <returns>Value</returns>
		TValue Get(TKey key);

		/// <summary>
		/// Retrieve the data by the given key if it is present
		/// </summary>
		/// <param name="key">Key</param>
		/// <param name="value">Value</param>
		/// <returns>Was the data present</returns>
		bool TryGet(TKey key, out TValue value);
		
		/// <summary>
		/// List the keys present in the repository
		/// </summary>
		/// <value>Keys</value>
		IEnumerable<TKey> Keys { get; }
	}
}