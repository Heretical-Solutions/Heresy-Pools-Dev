using System.Collections.Generic;

namespace HereticalSolutions.Repositories
{
    /// <summary>
    /// Repository interface
    /// </summary>
    /// <typeparam name="TKey">Key data type</typeparam>
    /// <typeparam name="TValue">Value data type</typeparam>
    public interface IRepository<TKey, TValue>
    {
        /// <summary>
        /// Does the repository have the data by the given key?
        /// </summary>
        /// <param name="key">Key</param>
        /// <returns>Does it or not</returns>
        bool Has(TKey key);

        /// <summary>
        /// Add the given data by the given key
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="value">Value</param>
        void Add(TKey key, TValue value);
        
        /// <summary>
        /// Update the data by the given key
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="value">Value</param>
        void Update(TKey key, TValue value);

        /// <summary>
        /// Set the data by the given key
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="value">Value</param>
        void AddOrUpdate(TKey key, TValue value);       
        
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
        /// Remove the data by the given key
        /// </summary>
        /// <param name="key">Key</param>
        void Remove(TKey key);
        
        /// <summary>
        /// List the keys present in the repository
        /// </summary>
        /// <value>Keys</value>
        IEnumerable<TKey> Keys { get; }
    }
}