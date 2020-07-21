using System;
using System.Threading.Tasks;
using Expensely.Domain.Core.Primitives;
using Microsoft.EntityFrameworkCore;

namespace Expensely.Application.Abstractions
{
    public interface IDbContext
    {
        DbSet<TEntity> Set<TEntity>()
            where TEntity : Entity;

        Task<TEntity?> GetBydIdAsync<TEntity>(Guid id)
            where TEntity : Entity;

        void Insert<TEntity>(TEntity entity)
            where TEntity : Entity;

        void Remove<TEntity>(TEntity entity)
            where TEntity : Entity;
    }
}