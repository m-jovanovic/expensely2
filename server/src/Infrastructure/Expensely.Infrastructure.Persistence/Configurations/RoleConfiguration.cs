using Expensely.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Expensely.Infrastructure.Persistence.Configurations
{
    /// <summary>
    /// Represents the <see cref="Role"/> entity configuration.
    /// </summary>
    internal sealed class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        /// <inheritdoc />
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable("Role");

            builder.HasKey(r => r.Name);

            builder.Property(r => r.Name).HasColumnType("nvarchar(50)").IsRequired();

            builder.Property(r => r.Description).HasColumnType("nvarchar(200)").IsRequired(false);

            builder.Property(r => r.Permissions).HasField("_permissions").HasColumnType("int[]").IsRequired();

            builder.Ignore(r => r.Permissions);
        }
    }
}
