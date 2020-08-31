using System;
using System.Threading.Tasks;
using Expensely.Application.Core.Abstractions.Data;
using Expensely.Domain.Core;

namespace Expensely.Infrastructure.Persistence.IntegrationTests.Common
{
    public abstract class DbContextTest : IDisposable
    {
        private readonly ExpenselyDbContext _dbContext;

        protected DbContextTest()
        {
            _dbContext = DbContextFactory.Create();
        }

        public void Dispose()
        {
            DbContextFactory.Destroy(_dbContext);
        }

        public IDbContext DbContext => _dbContext;

        protected async Task<TEntity> InsertAsync<TEntity>(TEntity entity)
            where TEntity : Entity
        {
            _dbContext.Insert(entity);

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
            => await _dbContext.GetBydIdAsync<TEntity>(id);
    }
}
