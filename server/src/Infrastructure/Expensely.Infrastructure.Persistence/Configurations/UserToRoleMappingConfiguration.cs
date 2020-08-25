using Expensely.Domain.Authorization;
using Expensely.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Expensely.Infrastructure.Persistence.Configurations
{
    /// <summary>
    /// Represents the <see cref="UserRole"/> entity configuration.
    /// </summary>
    internal sealed class UserToRoleMappingConfiguration : IEntityTypeConfiguration<UserRole>
    {
        /// <inheritdoc />
        public void Configure(EntityTypeBuilder<UserRole> builder)
        {
            builder.ToTable("UserToRoleMapping");

            builder.HasKey(m => new { m.UserId, m.RoleName });
        }
    }
}
