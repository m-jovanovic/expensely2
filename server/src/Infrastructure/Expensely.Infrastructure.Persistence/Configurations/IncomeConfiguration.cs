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
            builder.ToTable(TableNames.Transactions);

            builder.HasKey(income => income.Id);

            builder.Property(income => income.Name).HasMaxLength(100).IsRequired();

            builder.OwnsOne(income => income.Money, moneyBuilder =>
            {
                moneyBuilder.WithOwner();

                moneyBuilder.Property(money => money.Amount).HasColumnName("amount").HasColumnType("numeric(19,4)").IsRequired();

                // TODO: Only persist currency code in the database and add a value converter.
                moneyBuilder.OwnsOne(money => money.Currency, currencyBuilder =>
                {
                    currencyBuilder.WithOwner();

                    currencyBuilder.Property(currency => currency.Name).HasColumnName("currency_name").IsRequired();

                    currencyBuilder.Property(currency => currency.Code)
                        .HasColumnName("currency_code")
                        .HasMaxLength(3)
                        .IsRequired();

                    currencyBuilder.Property(currency => currency.Symbol)
                        .HasColumnName("currency_symbol")
                        .HasMaxLength(5)
                        .IsRequired();
                });
            });

            builder.Property(income => income.TransactionType).IsRequired();

            builder.Property(income => income.OccurredOn).HasColumnType("date").IsRequired();

            builder.Property(income => income.CreatedOnUtc).HasColumnType("timestamp").IsRequired();

            builder.Property(income => income.ModifiedOnUtc).HasColumnType("timestamp").IsRequired(false);

            builder.Property(income => income.DeletedOnUtc).HasColumnType("timestamp").IsRequired(false);

            builder.Property(income => income.Deleted).HasDefaultValue(false).IsRequired();

            builder.HasQueryFilter(income => !income.Deleted && income.TransactionType == TransactionType.Income);
        }
    }
}
