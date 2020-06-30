using Expensely.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Expensely.Persistence.Configurations
{
    internal sealed class ExpenseConfiguration : IEntityTypeConfiguration<Expense>
    {
        /// <inheritdoc />
        public void Configure(EntityTypeBuilder<Expense> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Amount).HasColumnType("decimal(19,4)").IsRequired();

            builder.Property(e => e.CreatedOnUtc).HasColumnType("datetime2(7)").IsRequired();

            builder.Property(e => e.ModifiedOnUtc).HasColumnType("datetime2(7)").IsRequired(false);

            builder.Property(e => e.Deleted).HasDefaultValue(false).IsRequired();
        }
    }
}
