using Expensely.Domain.Primitives;
using Microsoft.EntityFrameworkCore;

namespace Expensely.Application.Interfaces
{
    public interface IDbContext
    {
        DbSet<TEntity> Set<TEntity>()
            where TEntity : Entity;

        void Insert<TEntity>(TEntity entity)
            where TEntity : Entity;
    }
}