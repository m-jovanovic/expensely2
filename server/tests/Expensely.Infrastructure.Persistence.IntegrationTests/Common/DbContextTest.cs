using System;
using System.Threading.Tasks;
using Expensely.Application.Abstractions.Data;
using Expensely.Domain.Core.Primitives;
using Microsoft.EntityFrameworkCore;

namespace Expensely.Infrastructure.Persistence.IntegrationTests.Common
{
    public abstract class DbContextTest : IDisposable
    {
        private readonly ExpenselyDbContext _dbContext;

        protected DbContextTest()
        {
            _dbContext = ExpenselyDbContextFactory.Create();
        }

        public void Dispose()
        {
            ExpenselyDbContextFactory.Destroy(_dbContext);
        }

        public IDbContext DbContext => _dbContext;

        protected async Task<TEntity> AddAsync<TEntity>(TEntity entity)
            where TEntity : Entity
        {
            _dbContext.Add(entity);

            await _dbContext.SaveChangesAsync();

            return entity;
        }

        protected async Task<TEntity> UpdateAsync<TEntity>(TEntity entity)
            where TEntity : Entity
        {
            _dbContext.Update(entity);

            await _dbContext.SaveChangesAsync();

            return entity;
        }

        protected async Task RemoveAsync<TEntity>(TEntity entity)
            where TEntity : Entity
        {
            _dbContext.Remove(entity);

            await _dbContext.SaveChangesAsync();
        }

        protected async Task SaveChangesAsync() => await _dbContext.SaveChangesAsync();

        protected async Task<TEntity?> FindAsync<TEntity>(Guid id)
            where TEntity : Entity
            => await _dbContext.Set<TEntity>().FirstOrDefaultAsync<TEntity>(x => x.Id == id);
    }
}
