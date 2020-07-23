using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Expensely.Domain.Core.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Expensely.Infrastructure.Persistence.Extensions
{
    /// <summary>
    /// Contains extensions methods for the <see cref="ModelBuilder"/> class.
    /// </summary>
    internal static class ModelBuilderExtensions
    {
        private static readonly ValueConverter<DateTime, DateTime> UtcValueConverter =
            new ValueConverter<DateTime, DateTime>(outside => outside, inside => DateTime.SpecifyKind(inside, DateTimeKind.Utc));

        private static readonly MethodInfo SetSoftDeleteFilterMethod = typeof(ModelBuilderExtensions)
            .GetMethods(BindingFlags.Static | BindingFlags.NonPublic)
            .Single(m => m.IsGenericMethod && m.Name == nameof(SetSoftDeleteFilter));

        /// <summary>
        /// Applies the UTC date-time converter to all of the properties that are <see cref="DateTime"/> and end with Utc.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        internal static void ApplyUtcDateTimeConverter(this ModelBuilder modelBuilder)
        {
            foreach (IMutableEntityType mutableEntityType in modelBuilder.Model.GetEntityTypes())
            {
                IEnumerable<IMutableProperty> dateTimeUtcProperties = mutableEntityType.GetProperties()
                    .Where(p => p.ClrType == typeof(DateTime) && p.Name.EndsWith("Utc", StringComparison.Ordinal));

                foreach (IMutableProperty mutableProperty in dateTimeUtcProperties)
                {
                    mutableProperty.SetValueConverter(UtcValueConverter);
                }
            }
        }

        /// <summary>
        /// Applies the soft delete query filter to all entities implementing <see cref="ISoftDeletableEntity"/> interface.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        internal static void ApplySoftDeleteQueryFilter(this ModelBuilder modelBuilder)
        {
            IEnumerable<IMutableEntityType> softDeletableEntities = modelBuilder.Model.GetEntityTypes()
                .Where(e => typeof(ISoftDeletableEntity).IsAssignableFrom(e.ClrType));

            foreach (IMutableEntityType mutableEntityType in softDeletableEntities)
            {
                modelBuilder.SetSoftDeleteFilter(mutableEntityType.ClrType);
            }
        }

        /// <summary>
        /// Applies the soft delete query filter to the specified entity type.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        /// <param name="entityType">The entity type.</param>
        internal static void SetSoftDeleteFilter(this ModelBuilder modelBuilder, Type entityType)
        {
            SetSoftDeleteFilterMethod.MakeGenericMethod(entityType).Invoke(null, new object[] { modelBuilder });
        }

        /// <summary>
        /// Applies the soft delete query filter to the specified entity type.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <param name="modelBuilder">The model builder.</param>
        private static void SetSoftDeleteFilter<TEntity>(this ModelBuilder modelBuilder)
            where TEntity : class, ISoftDeletableEntity
        {
            modelBuilder.Entity<TEntity>().HasQueryFilter(x => x.Deleted == false);
        }
    }
}
