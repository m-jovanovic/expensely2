using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;

namespace Expensely.Application.Interfaces
{
    public interface ISqlConnectionFactory
    {
        Task<SqlConnection> CreateSqlConnectionAsync(CancellationToken cancellationToken = default);
    }
}
