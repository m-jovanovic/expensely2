using Expensely.Infrastructure.Authentication.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Expensely.Infrastructure.Authentication.Configurations
{
    internal class UserToPaidModulesMappingConfiguration : IEntityTypeConfiguration<UserToPaidModulesMapping>
    {
        public void Configure(EntityTypeBuilder<UserToPaidModulesMapping> builder)
        {
            builder.ToTable("UserToPaidModulesMapping");

            builder.HasKey(m => m.UserId);

            builder.Property(m => m.PaidModules).IsRequired();
        }
    }
}
