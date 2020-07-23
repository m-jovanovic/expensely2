using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;

namespace Expensely.Application.Abstractions
{
    /// <summary>
    /// Represents the SQL connection factory interface.
    /// </summary>
    public interface ISqlConnectionFactory
    {
        /// <summary>
        /// Creates a new <see cref="SqlConnection"/> instance.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The new <see cref="SqlConnection"/> instance.</returns>
        Task<SqlConnection> CreateSqlConnectionAsync(CancellationToken cancellationToken = default);
    }
}
