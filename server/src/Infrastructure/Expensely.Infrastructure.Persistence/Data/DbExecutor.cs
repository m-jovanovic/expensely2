using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Expensely.Application.Core.Abstractions.Data;

namespace Expensely.Infrastructure.Persistence.Data
{
    /// <summary>
    /// Represents the database executor.
    /// </summary>
    internal sealed class DbExecutor : IDbExecutor
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="DbExecutor"/> class.
        /// </summary>
        /// <param name="sqlConnectionFactory">The sql connection factory.</param>
        public DbExecutor(ISqlConnectionFactory sqlConnectionFactory) => _sqlConnectionFactory = sqlConnectionFactory;

        /// <inheritdoc />
        public async Task<T[]> QueryAsync<T>(string sql, object parameters)
        {
            using IDbConnection connection = await _sqlConnectionFactory.CreateSqlConnectionAsync();

            IEnumerable<T> result = await connection.QueryAsync<T>(sql, parameters);

            return result.ToArray();
        }
    }
}
