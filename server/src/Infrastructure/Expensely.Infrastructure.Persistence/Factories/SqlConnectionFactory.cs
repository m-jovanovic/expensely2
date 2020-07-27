using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Abstractions;
using Npgsql;

namespace Expensely.Infrastructure.Persistence.Factories
{
    /// <summary>
    /// Represents the SQL connection factory.
    /// </summary>
    internal sealed class SqlConnectionFactory : ISqlConnectionFactory
    {
        private readonly ConnectionString _connectionString;

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlConnectionFactory"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public SqlConnectionFactory(ConnectionString connectionString) => _connectionString = connectionString;

        /// <inheritdoc />
        public async Task<IDbConnection> CreateSqlConnectionAsync(CancellationToken cancellationToken)
        {
            var sqlConnection = new NpgsqlConnection(_connectionString);

            if (sqlConnection.State != ConnectionState.Open)
            {
                await sqlConnection.OpenAsync(cancellationToken);
            }

            return sqlConnection;
        }
    }
}
