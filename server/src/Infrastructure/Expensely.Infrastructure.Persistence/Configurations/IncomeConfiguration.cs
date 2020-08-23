using Expensely.Domain.Transactions;
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
        }
    }
}
