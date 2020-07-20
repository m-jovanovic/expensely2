using Expensely.Authentication.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Expensely.Authentication.Configurations
{
    internal class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable("Role");

            builder.HasKey(r => r.Name);

            builder.Property(r => r.Name).HasColumnType("nvarchar(50)").IsRequired();

            builder.Property(r => r.Description).HasColumnType("nvarchar(200)").IsRequired(false);

            builder.Property(r => r.Permissions).HasField("_permissions").HasColumnType("int[]").IsRequired();
        }
    }
}
