using Expensely.Authentication.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Expensely.Authentication.Configurations
{
    internal class UserToRoleMappingConfiguration : IEntityTypeConfiguration<UserToRoleMapping>
    {
        public void Configure(EntityTypeBuilder<UserToRoleMapping> builder)
        {
            builder.ToTable("UserToRoleMapping");

            builder.HasKey(m => new { m.UserId, m.RoleName });
        }
    }
}
