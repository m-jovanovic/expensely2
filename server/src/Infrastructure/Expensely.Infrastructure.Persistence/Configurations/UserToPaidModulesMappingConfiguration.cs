using Expensely.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Expensely.Infrastructure.Persistence.Configurations
{
    /// <summary>
    /// Represents the <see cref="UserToPaidModulesMapping"/> entity configuration.
    /// </summary>
    internal sealed class UserToPaidModulesMappingConfiguration : IEntityTypeConfiguration<UserToPaidModulesMapping>
    {
        /// <inheritdoc />
        public void Configure(EntityTypeBuilder<UserToPaidModulesMapping> builder)
        {
            builder.ToTable("UserToPaidModulesMapping");

            builder.HasKey(m => m.UserId);

            builder.Property(m => m.PaidModules).IsRequired();
        }
    }
}
