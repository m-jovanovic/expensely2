using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Expensely.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Expensely.Persistence.Extensions
{
    internal static class ModelBuilderExtensions
    {
        private static readonly ValueConverter<DateTime, DateTime> UtcValueConverter =
            new ValueConverter<DateTime, DateTime>(outside => outside, inside => DateTime.SpecifyKind(inside, DateTimeKind.Utc));

        private static readonly MethodInfo SetSoftDeleteFilterMethod = typeof(ModelBuilderExtensions)
            .GetMethods(BindingFlags.Static | BindingFlags.NonPublic)
            .Single(m => m.IsGenericMethod && m.Name == nameof(SetSoftDeleteFilter));

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

        internal static void ApplySoftDeleteQueryFilter(this ModelBuilder modelBuilder)
        {
            IEnumerable<IMutableEntityType> softDeletableEntities = modelBuilder.Model.GetEntityTypes()
                .Where(e => typeof(ISoftDeletableEntity).IsAssignableFrom(e.ClrType));

            foreach (IMutableEntityType mutableEntityType in softDeletableEntities)
            {
                modelBuilder.SetSoftDeleteFilter(mutableEntityType.ClrType);
            }
        }

        private static void SetSoftDeleteFilter(this ModelBuilder modelBuilder, Type entityType)
        {
            SetSoftDeleteFilterMethod.MakeGenericMethod(entityType).Invoke(null, new object[] { modelBuilder });
        }

        private static void SetSoftDeleteFilter<TEntity>(this ModelBuilder modelBuilder)
            where TEntity : class, ISoftDeletableEntity
        {
            modelBuilder.Entity<TEntity>().HasQueryFilter(x => x.Deleted == false);
        }
    }
}
