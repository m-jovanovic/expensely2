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

            builder.OwnsOne(m => m.Money, moneyBuilder =>
            {
                moneyBuilder.WithOwner();

                moneyBuilder.Property(m => m.Amount).HasColumnName("Amount").HasColumnType("decimal(19,4)").IsRequired();

                moneyBuilder.OwnsOne(m => m.Currency, currencyBuilder =>
                {
                    currencyBuilder.WithOwner();

                    currencyBuilder.Property(c => c.Id).HasColumnName("CurrencyId").IsRequired();

                    currencyBuilder.Property(c => c.Code)
                        .HasColumnName("CurrencyCode")
                        .HasColumnType("varchar(3)")
                        .HasMaxLength(3)
                        .IsRequired();

                    currencyBuilder.Property(c => c.Sign)
                        .HasColumnName("CurrencySign")
                        .HasColumnType("varchar(5)")
                        .HasMaxLength(5)
                        .IsRequired();
                });
            });

            builder.Property(e => e.CreatedOnUtc).HasColumnType("datetime2(7)").IsRequired();

            builder.Property(e => e.ModifiedOnUtc).HasColumnType("datetime2(7)").IsRequired(false);

            builder.Property(e => e.Deleted).HasDefaultValue(false).IsRequired();
        }
    }
}
