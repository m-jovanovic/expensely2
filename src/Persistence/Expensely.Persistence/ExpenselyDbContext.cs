using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Interfaces;
using Expensely.Domain;
using Expensely.Domain.Primitives;
using Expensely.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Expensely.Persistence
{
    public sealed class ExpenselyDbContext : DbContext, IDbContext, IUnitOfWork
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExpenselyDbContext"/> class.
        /// </summary>
        /// <param name="options">The database context options.</param>
        public ExpenselyDbContext(DbContextOptions options)
            : base(options)
        {
        }

        /// <inheritdoc />
        public new DbSet<TEntity> Set<TEntity>()
            where TEntity : Entity
        {
            return base.Set<TEntity>();
        }

        /// <inheritdoc />
        public void Insert<TEntity>(TEntity entity)
            where TEntity : Entity =>
            Set<TEntity>().Add(entity);

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            DateTime utcNow = DateTime.UtcNow;

            foreach (EntityEntry<IAuditableEntity> entityEntry in ChangeTracker.Entries<IAuditableEntity>())
            {
                if (entityEntry.State == EntityState.Added)
                {
                    entityEntry.Property(nameof(IAuditableEntity.CreatedOnUtc)).CurrentValue = utcNow;
                }

                if (entityEntry.State == EntityState.Modified)
                {
                    entityEntry.Property(nameof(IAuditableEntity.ModifiedOnUtc)).CurrentValue = utcNow;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }

        /// <inheritdoc />
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyUtcDateTimeConverter();

            modelBuilder.ApplySoftDeleteQueryFilter();

            // Apply configurations after registering soft delete query filter, because only the last query filter is considered.
            // Some configurations could have a separate query filter with additional conditions.
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(modelBuilder);
        }
    }
}
