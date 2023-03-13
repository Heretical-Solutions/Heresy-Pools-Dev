using System;
using System.Collections.Generic;
using HereticalSolutions.Repositories.Factories;

namespace HereticalSolutions.Repositories
{
	/// <summary>
	/// Repository that stores object data in a type-object key value repository
	/// </summary>
	public class DictionaryObjectRepository :
		IObjectRepository,
		IReadOnlyObjectRepository,
		ICloneableObjectRepository
	{
		/// <summary>
		/// Actual storage
		/// </summary>
		private readonly IRepository<Type, object> database;

		public DictionaryObjectRepository(IRepository<Type, object> database)
		{
			this.database = database;
		}

		#region IObjectRepository

		#region IReadOnlyObjectRepository
		
		public bool Has<TValue>()
		{
			return database.Has(typeof(TValue));
		}

		public bool Has(Type valueType)
		{
			return database.Has(valueType);
		}

		public TValue Get<TValue>()
		{
			return (TValue)database.Get(typeof(TValue));
		}

		public object Get(Type valueType)
		{
			return database.Get(valueType);
		}

		public bool TryGet<TValue>(out TValue value)
		{
			value = default(TValue);
			
			bool result = database.TryGet(typeof(TValue), out object valueObject);

			if (result)
				value = (TValue)valueObject;

			return result;
		}
		
		public bool TryGet(Type valueType, out object value)
		{
			return database.TryGet(valueType, out value);
		}

		public IEnumerable<Type> Keys
		{
			get => database.Keys;
		}
		
		#endregion
		
		public void Add<TValue>(TValue value)
		{
			database.Add(typeof(TValue), value);
		}

		public void Add(Type valueType, object value)
		{
			database.AddOrUpdate(valueType, value);
		}
		
		public void Update<TValue>(TValue value)
		{
			database.Update(typeof(TValue), value);
		}

		public void Update(Type valueType, object value)
		{
			database.Update(valueType, value);
		}

		public void AddOrUpdate<TValue>(TValue value)
		{
			database.AddOrUpdate(typeof(TValue), value);
		}

		public void AddOrUpdate(Type valueType, object value)
		{
			database.AddOrUpdate(valueType, value);
		}

		public void Remove<TValue>()
		{
			database.Remove(typeof(TValue));
		}

		public void Remove(Type valueType)
		{
			database.Remove(valueType);
		}

		#endregion

		#region IClonable
		
		public IObjectRepository Clone()
		{
			return RepositoriesFactory.CloneDictionaryObjectRepository(database);
		}
		
		#endregion
	}
}