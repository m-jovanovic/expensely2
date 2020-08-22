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

            builder.HasQueryFilter(transaction => !transaction.Deleted);
        }
    }
}
