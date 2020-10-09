using Expensely.Domain.Transactions;
using Expensely.Infrastructure.Persistence.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Expensely.Infrastructure.Persistence.Configurations
{
    /// <summary>
    /// Contains the <see cref="Transaction"/> entity configuration.
    /// </summary>
    internal sealed class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
    {
        /// <inheritdoc />
        public void Configure(EntityTypeBuilder<Transaction> builder)
        {
            builder.ToTable(TableNames.Transactions)
                .HasDiscriminator(transaction => transaction.TransactionType)
                .HasValue<Expense>(TransactionType.Expense)
                .HasValue<Income>(TransactionType.Income);

            builder.Property(transaction => transaction.Name).HasMaxLength(100).IsRequired();

            builder.OwnsOne(transaction => transaction.Money, moneyBuilder =>
            {
                moneyBuilder.WithOwner();

                moneyBuilder.Property(money => money.Amount).HasColumnName("amount").HasColumnType("numeric(19,4)").IsRequired();

                moneyBuilder.OwnsOne(money => money.Currency, currencyBuilder =>
                {
                    currencyBuilder.WithOwner();

                    currencyBuilder.Property(currency => currency.Value).HasColumnName("currency").IsRequired();

                    currencyBuilder.Ignore(currency => currency.Code);

                    currencyBuilder.Ignore(currency => currency.Name);
                });
            });

            builder.Property(transaction => transaction.TransactionType).IsRequired();

            builder.Property(transaction => transaction.OccurredOn).HasColumnType("date").IsRequired();

            builder.Property(transaction => transaction.CreatedOnUtc).HasColumnType("timestamp").IsRequired();

            builder.Property(transaction => transaction.ModifiedOnUtc).HasColumnType("timestamp").IsRequired(false);

            builder.Property(transaction => transaction.DeletedOnUtc).HasColumnType("timestamp").IsRequired(false);

            builder.Property(transaction => transaction.Deleted).HasDefaultValue(false).IsRequired();

            builder.HasQueryFilter(transaction => !transaction.Deleted);
        }
    }
}
