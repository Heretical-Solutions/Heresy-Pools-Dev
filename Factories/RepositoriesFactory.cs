using System;
using System.Collections.Generic;

namespace HereticalSolutions.Repositories.Factories
{
	public static partial class RepositoriesFactory
	{
		#region Dictionary object repository

		public static DictionaryObjectRepository BuildDictionaryObjectRepository()
		{
			return new DictionaryObjectRepository(
				BuildDictionaryRepository<Type, object>());
		}
		
		public static DictionaryObjectRepository BuildDictionaryObjectRepository(
			IRepository<Type, object> database)
		{
			return new DictionaryObjectRepository(
				database);
		}
		
		public static DictionaryObjectRepository CloneDictionaryObjectRepository(
			IRepository<Type, object> contents)
		{
			return new DictionaryObjectRepository(
				((IClonableRepository<Type, object>)contents).Clone());
		}

		#endregion
		
		#region Dictionary repository
		
		public static DictionaryRepository<TKey, TValue> BuildDictionaryRepository<TKey, TValue>()
		{
			return new DictionaryRepository<TKey, TValue>(
				new Dictionary<TKey, TValue>());
		}
		
		public static DictionaryRepository<TKey, TValue> BuildDictionaryRepository<TKey, TValue>(
			Dictionary<TKey, TValue> database)
		{
			return new DictionaryRepository<TKey, TValue>(
				database);
		}
		
		public static DictionaryRepository<TKey, TValue> CloneDictionaryRepository<TKey, TValue>(
			Dictionary<TKey, TValue> contents)
		{
			return new DictionaryRepository<TKey, TValue>(
				new Dictionary<TKey, TValue>(contents));
		}
		
		#endregion
	}
}