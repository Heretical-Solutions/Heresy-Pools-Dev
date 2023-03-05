using System.Collections.Generic;

namespace HereticalSolutions.Repositories
{
	/// <summary>
	/// Repository that stores data in a generic dictionary
	/// </summary>
	/// <typeparam name="TKey">Key data type</typeparam>
	/// <typeparam name="TValue">Value data type</typeparam>
	public class DictionaryRepository<TKey, TValue> :
		IRepository<TKey, TValue>,
		IReadOnlyRepository<TKey, TValue>
	{
		/// <summary>
		/// Actual storage
		/// </summary>
		protected Dictionary<TKey, TValue> database;

		public DictionaryRepository(Dictionary<TKey, TValue> database)
		{
			this.database = database;
		}

		/// <summary>
		/// Does the repository have the data by the given key?
		/// </summary>
		/// <param name="key">Key</param>
		/// <returns>Does it or not</returns>
		public bool Has(TKey key)
		{
			return database.ContainsKey(key);
		}

		/// <summary>
		/// Add the given data by the given key
		/// </summary>
		/// <param name="key">Key</param>
		/// <param name="value">Value</param>
		public void Add(TKey key, TValue value)
		{
			database.Add(key, value);
		}

		/// <summary>
		/// Update the data by the given key
		/// </summary>
		/// <param name="key">Key</param>
		/// <param name="value">Value</param>
		public void Update(TKey key, TValue value)
		{
			database[key] = value;
		}

		/// <summary>
		/// Set the data by the given key
		/// </summary>
		/// <param name="key">Key</param>
		/// <param name="value">Value</param>
		public void AddOrUpdate(TKey key, TValue value)
		{
			if (Has(key))
				Update(key, value);
			else
				Add(key, value);
		}

		/// <summary>
		/// Retrieve the data by the given key
		/// </summary>
		/// <param name="key">Key</param>
		/// <returns>Value</returns>
		public TValue Get(TKey key)
		{
			return database[key];
		}

		/// <summary>
		/// Retrieve the data by the given key if it is present
		/// </summary>
		/// <param name="key">Key</param>
		/// <param name="value">Value</param>
		/// <returns>Was the data present</returns>
		public bool TryGet(TKey key, out TValue value)
		{
			return database.TryGetValue(key, out value);
		}

		/// <summary>
		/// Remove the data by the given key
		/// </summary>
		/// <param name="key">Key</param>
		public void Remove(TKey key)
		{
			database.Remove(key);
		}

		/// <summary>
		/// List the keys present in the repository
		/// </summary>
		/// <value>Keys</value>
		public IEnumerable<TKey> Keys { get { return database.Keys; } }
	}
}