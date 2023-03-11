using System;
using System.Collections.Generic;

namespace HereticalSolutions.Repositories
{
    /// <summary>
    /// Object repository interface
    /// </summary>
    public interface IObjectRepository
    {
        /// <summary>
        /// Does the repository have the data by the given type?
        /// </summary>
        /// <typeparam name="TValue">Value data type</typeparam>
        /// <returns>Does it or not</returns>
        bool Has<TValue>();

        /// <summary>
        /// Does the repository have the data by the given type?
        /// </summary>
        /// <param name="valueType">Value data type</param>
        /// <returns>Does it or not</returns>
        bool Has(Type valueType);

        /// <summary>
        /// Add the given data by the given type
        /// </summary>
        /// <param name="value">Value</param>
        /// <typeparam name="TValue">Value data type</typeparam>
        void Add<TValue>(TValue value);

        /// <summary>
        /// Add the given data by the given type
        /// </summary>
        /// <param name="valueType">Value data type</param>
        /// <param name="value">value</param>
        void Add(Type valueType, object value);
        
        /// <summary>
        /// Update the data by the given type
        /// </summary>
        /// <param name="value">Value</param>
        /// <typeparam name="TValue">Value data type</typeparam>
        void Update<TValue>(TValue value);

        /// <summary>
        /// Update the data by the given type
        /// </summary>
        /// <param name="valueType">Value data type</param>
        /// <param name="value">Value</param>
        void Update(Type valueType, object value);

        /// <summary>
        /// Set the data by the given type
        /// </summary>
        /// <param name="value">Value</param>
        /// <typeparam name="TValue">Value data type</typeparam>
        void AddOrUpdate<TValue>(TValue value);

        /// <summary>
        /// Set the data by the given type
        /// </summary>
        /// <param name="valueType">Value</param>
        /// <param name="value">Value data type</param>
        void AddOrUpdate(Type valueType, object value);

        /// <summary>
        /// Retrieve the data by the given type
        /// </summary>
        /// <typeparam name="TValue">Value data type</typeparam>
        /// <returns>Value</returns>
        TValue Get<TValue>();

        /// <summary>
        /// Retrieve the data by the given type
        /// </summary>
        /// <param name="valueType">Value data type</param>
        /// <returns>Value</returns>
        object Get(Type valueType);
        
        /// <summary>
        /// Retrieve the data by the given type if it is present
        /// </summary>
        /// <param name="value">Value</param>
        /// <typeparam name="TValue">Value data type</typeparam>
        /// <returns>Was the data present</returns>
        bool TryGet<TValue>(out TValue value);

        /// <summary>
        /// Retrieve the data by the given type if it is present
        /// </summary>
        /// <param name="valueType">Value data type</param>
        /// <param name="value">Value</param>
        /// <returns>Was the data present</returns>
        bool TryGet(Type valueType, out object value);

        /// <summary>
        /// Remove the data by the given type
        /// </summary>
        /// <typeparam name="TValue">Value data type</typeparam>
        void Remove<TValue>();

        /// <summary>
        /// Remove the data by the given type
        /// </summary>
        /// <param name="valueType">Value data type</param>
        void Remove(Type valueType);
        
        /// <summary>
        /// List the types present in the repository
        /// </summary>
        /// <value>Keys</value>
        IEnumerable<Type> Keys { get; }
    }
}