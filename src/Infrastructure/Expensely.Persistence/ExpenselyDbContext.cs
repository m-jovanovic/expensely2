using System;
using System.Linq;
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
        public async Task<TEntity?> GetBydIdAsync<TEntity>(Guid id)
            where TEntity : Entity
        {
            if (id == Guid.Empty)
            {
                return null;
            }

            return await Set<TEntity>().FirstOrDefaultAsync(e => e.Id == id);
        }

        /// <inheritdoc />
        public void Insert<TEntity>(TEntity entity)
            where TEntity : Entity =>
            Set<TEntity>().Add(entity);

        /// <inheritdoc />
        public new void Remove<TEntity>(TEntity entity)
            where TEntity : Entity =>
            Set<TEntity>().Remove(entity);

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            DateTime utcNow = DateTime.UtcNow;

            UpdateAuditableEntities(utcNow);

            UpdateSoftDeletableEntities();

            return base.SaveChangesAsync(cancellationToken);
        }

        /// <inheritdoc />
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            modelBuilder.ApplyUtcDateTimeConverter();

            modelBuilder.ApplySoftDeleteQueryFilter();

            base.OnModelCreating(modelBuilder);
        }

        private void UpdateAuditableEntities(DateTime utcNow)
        {
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
        }

        private void UpdateSoftDeletableEntities()
        {
            foreach (EntityEntry<ISoftDeletableEntity> entityEntry in ChangeTracker.Entries<ISoftDeletableEntity>())
            {
                if (entityEntry.State == EntityState.Deleted)
                {
                    entityEntry.Property(nameof(ISoftDeletableEntity.Deleted)).CurrentValue = true;

                    entityEntry.State = EntityState.Modified;

                    UpdateDeletedEntityEntryReferencesToUnchanged(entityEntry);
                }
            }
        }

        /// <summary>
        /// Updates the specified entity entry's referenced entries in the deleted state to the modified state.
        /// This method is recursive.
        /// </summary>
        /// <param name="entityEntry">The entity entry.</param>
        private static void UpdateDeletedEntityEntryReferencesToUnchanged(EntityEntry entityEntry)
        {
            if (!entityEntry.References.Any())
            {
                return;
            }

            foreach (ReferenceEntry referenceEntry in entityEntry.References.Where(r => r.TargetEntry.State == EntityState.Deleted))
            {
                referenceEntry.TargetEntry.State = EntityState.Unchanged;

                UpdateDeletedEntityEntryReferencesToUnchanged(referenceEntry.TargetEntry);
            }
        }
    }
}
