using Expensely.Infrastructure.Authentication.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Expensely.Infrastructure.Authentication.Configurations
{
    /// <summary>
    /// Represents the <see cref="UserToRoleMapping"/> entity configuration.
    /// </summary>
    internal sealed class UserToRoleMappingConfiguration : IEntityTypeConfiguration<UserToRoleMapping>
    {
        /// <inheritdoc />
        public void Configure(EntityTypeBuilder<UserToRoleMapping> builder)
        {
            builder.ToTable("UserToRoleMapping");

            builder.HasKey(m => new { m.UserId, m.RoleName });
        }
    }
}
