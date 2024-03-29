﻿using System.Threading.Tasks;

namespace Expensely.Application.Core.Abstractions.Data
{
    /// <summary>
    /// Represents the database executor interface.
    /// </summary>
    public interface IDbExecutor
    {
        /// <summary>
        /// Executes the specified sql query with the specified parameters and returns an array of the specified type.
        /// </summary>
        /// <typeparam name="T">The type of the result.</typeparam>
        /// <param name="sql">The sql query.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>The array of the specified type.</returns>
        Task<T[]> QueryAsync<T>(string sql, object parameters);
    }
}