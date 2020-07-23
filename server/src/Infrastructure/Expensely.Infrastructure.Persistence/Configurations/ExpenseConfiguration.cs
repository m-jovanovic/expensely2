using Expensely.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Expensely.Infrastructure.Persistence.Configurations
{
    /// <summary>
    /// Contains the <see cref="Expense"/> entity configuration.
    /// </summary>
    internal sealed class ExpenseConfiguration : IEntityTypeConfiguration<Expense>
    {
        /// <inheritdoc />
        public void Configure(EntityTypeBuilder<Expense> builder)
        {
            builder.ToTable("Expense");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Name).HasColumnType("varchar(100)").IsRequired();

            builder.OwnsOne(m => m.Money, moneyBuilder =>
            {
                moneyBuilder.WithOwner();

                moneyBuilder.Property(m => m.Amount).HasColumnName("Amount").HasColumnType("numeric(19,4)").IsRequired();

                moneyBuilder.OwnsOne(m => m.Currency, currencyBuilder =>
                {
                    currencyBuilder.WithOwner();

                    currencyBuilder.Property(c => c.Id).HasColumnName("CurrencyId").IsRequired();

                    currencyBuilder.Property(c => c.Code)
                        .HasColumnName("CurrencyCode")
                        .HasColumnType("varchar(3)")
                        .HasMaxLength(3)
                        .IsRequired();

                    currencyBuilder.Property(c => c.Symbol)
                        .HasColumnName("CurrencySign")
                        .HasColumnType("varchar(5)")
                        .HasMaxLength(5)
                        .IsRequired();
                });
            });

            builder.Property(e => e.Date).HasColumnType("date").IsRequired();

            builder.Property(e => e.CreatedOnUtc).HasColumnType("timestamp").IsRequired();

            builder.Property(e => e.ModifiedOnUtc).HasColumnType("timestamp").IsRequired(false);

            builder.Property(e => e.DeletedOnUtc).HasColumnType("timestamp").IsRequired(false);

            builder.Property(e => e.Deleted).HasDefaultValue(false).IsRequired();
        }
    }
}
