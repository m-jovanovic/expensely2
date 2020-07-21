using System.Threading;
using System.Threading.Tasks;

namespace Expensely.Application.Abstractions
{
    public interface IUnitOfWork
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
