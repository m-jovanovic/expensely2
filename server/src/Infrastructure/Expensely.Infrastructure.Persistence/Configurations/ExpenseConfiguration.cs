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
            builder.ToTable("transactions");

            builder.HasKey(expense => expense.Id);

            builder.Property(expense => expense.Name).HasMaxLength(100).IsRequired();

            builder.OwnsOne(expense => expense.Money, moneyBuilder =>
            {
                moneyBuilder.WithOwner();

                moneyBuilder.Property(money => money.Amount).HasColumnName("amount").HasColumnType("numeric(19,4)").IsRequired();

                moneyBuilder.OwnsOne(money => money.Currency, currencyBuilder =>
                {
                    currencyBuilder.WithOwner();

                    currencyBuilder.Property(currency => currency.Id).HasColumnName("currency_id").IsRequired();

                    currencyBuilder.Property(currency => currency.Code)
                        .HasColumnName("currency_code")
                        .HasMaxLength(3)
                        .IsRequired();

                    currencyBuilder.Property(currency => currency.Sign)
                        .HasColumnName("currency_sign")
                        .HasMaxLength(5)
                        .IsRequired();
                });
            });

            builder.Property(expense => expense.TransactionType).IsRequired();

            builder.Property(expense => expense.OccurredOn).HasColumnType("date").IsRequired();

            builder.Property(expense => expense.CreatedOnUtc).HasColumnType("timestamp").IsRequired();

            builder.Property(expense => expense.ModifiedOnUtc).HasColumnType("timestamp").IsRequired(false);

            builder.Property(expense => expense.DeletedOnUtc).HasColumnType("timestamp").IsRequired(false);

            builder.Property(expense => expense.Deleted).HasDefaultValue(false).IsRequired();

            builder.HasQueryFilter(expense => !expense.Deleted && expense.TransactionType == TransactionType.Expense);
        }
    }
}
