using Expensely.Domain.Authorization;
using Expensely.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Expensely.Infrastructure.Persistence.Configurations
{
    /// <summary>
    /// Represents the <see cref="UserPaidModules"/> entity configuration.
    /// </summary>
    internal sealed class UserToPaidModulesMappingConfiguration : IEntityTypeConfiguration<UserPaidModules>
    {
        /// <inheritdoc />
        public void Configure(EntityTypeBuilder<UserPaidModules> builder)
        {
            builder.ToTable("UserToPaidModulesMapping");

            builder.HasKey(m => m.UserId);

            builder.Property(m => m.PaidModules).IsRequired();
        }
    }
}
