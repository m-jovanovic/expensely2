using Expensely.Domain.Transactions;
using Expensely.Infrastructure.Persistence.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Expensely.Infrastructure.Persistence.Configurations
{
    /// <summary>
    /// Contains the <see cref="Income"/> entity configuration.
    /// </summary>
    internal sealed class IncomeConfiguration : IEntityTypeConfiguration<Income>
    {
        /// <inheritdoc />
        public void Configure(EntityTypeBuilder<Income> builder)
        {
            builder.HasKey(income => income.Id);

            builder.Property(income => income.Name).HasMaxLength(100).IsRequired();

            builder.OwnsOne(income => income.Money, moneyBuilder =>
            {
                moneyBuilder.WithOwner();

                moneyBuilder.Property(money => money.Amount).HasColumnName("amount").HasColumnType("numeric(19,4)").IsRequired();

                moneyBuilder.Property(money => money.Currency)
                    .HasColumnName("currency")
                    .HasConversion(
                        currency => currency.Value,
                        currencyId => Currency.FromValue(currencyId))
                    .IsRequired();
            });

            builder.Property(income => income.TransactionType).IsRequired();

            builder.Property(income => income.OccurredOn).HasColumnType("date").IsRequired();

            builder.Property(income => income.CreatedOnUtc).HasColumnType("timestamp").IsRequired();

            builder.Property(income => income.ModifiedOnUtc).HasColumnType("timestamp").IsRequired(false);

            builder.Property(income => income.DeletedOnUtc).HasColumnType("timestamp").IsRequired(false);

            builder.Property(income => income.Deleted).HasDefaultValue(false).IsRequired();
        }
    }
}
