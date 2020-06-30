using System.Data;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Interfaces;

namespace Expensely.Persistence.Factories
{
    internal sealed class SqlConnectionFactory : ISqlConnectionFactory
    {
        private readonly ConnectionString _connectionString;

        public SqlConnectionFactory(ConnectionString connectionString)
        {
            _connectionString = connectionString;
        }

        /// <inheritdoc />
        public async Task<SqlConnection> CreateSqlConnectionAsync(CancellationToken cancellationToken)
        {
            var sqlConnection = new SqlConnection(_connectionString);

            if (sqlConnection.State != ConnectionState.Open)
            {
                await sqlConnection.OpenAsync(cancellationToken);
            }

            return sqlConnection;
        }
    }
}
