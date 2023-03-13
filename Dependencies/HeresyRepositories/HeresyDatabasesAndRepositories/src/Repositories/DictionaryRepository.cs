using System.Collections.Generic;
using HereticalSolutions.Repositories.Factories;

namespace HereticalSolutions.Repositories
{
	/// <summary>
	/// Repository that stores data in a generic dictionary
	/// </summary>
	/// <typeparam name="TKey">Key data type</typeparam>
	/// <typeparam name="TValue">Value data type</typeparam>
	public class DictionaryRepository<TKey, TValue> :
		IRepository<TKey, TValue>,
		IReadOnlyRepository<TKey, TValue>,
		IClonableRepository<TKey, TValue>
	{
		/// <summary>
		/// Actual storage
		/// </summary>
		private readonly Dictionary<TKey, TValue> database;

		public DictionaryRepository(Dictionary<TKey, TValue> database)
		{
			this.database = database;
		}

		#region IRepository

		#region IReadOnlyRepository
		
		public bool Has(TKey key)
		{
			return database.ContainsKey(key);
		}
		
		public TValue Get(TKey key)
		{
			return database[key];
		}
		
		public bool TryGet(TKey key, out TValue value)
		{
			return database.TryGetValue(key, out value);
		}
		
		public IEnumerable<TKey> Keys { get { return database.Keys; } }
		
		#endregion
		
		public void Add(TKey key, TValue value)
		{
			database.Add(key, value);
		}
		
		public void Update(TKey key, TValue value)
		{
			database[key] = value;
		}
		
		public void AddOrUpdate(TKey key, TValue value)
		{
			if (Has(key))
				Update(key, value);
			else
				Add(key, value);
		}

		public void Remove(TKey key)
		{
			database.Remove(key);
		}

		#endregion
		
		#region IClonableRepository
		
		public IRepository<TKey, TValue> Clone()
		{
			return RepositoriesFactory.CloneDictionaryRepository(database);
		}
		
		#endregion
	}
}