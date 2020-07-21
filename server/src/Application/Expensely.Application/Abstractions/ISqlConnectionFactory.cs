using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;

namespace Expensely.Application.Abstractions
{
    public interface ISqlConnectionFactory
    {
        Task<SqlConnection> CreateSqlConnectionAsync(CancellationToken cancellationToken = default);
    }
}
