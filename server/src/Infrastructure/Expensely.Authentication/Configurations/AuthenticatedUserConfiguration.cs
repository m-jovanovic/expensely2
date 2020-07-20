using Expensely.Authentication.Entities;
using Expensely.Domain.Validators.Email;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Expensely.Authentication.Configurations
{
    internal class AuthenticatedUserConfiguration : IEntityTypeConfiguration<AuthenticatedUser>
    {
        /// <inheritdoc />
        public void Configure(EntityTypeBuilder<AuthenticatedUser> builder)
        {
            builder.ToTable("User");

            builder.HasKey(u => u.Id);

            builder.Property(u => u.FirstName).HasColumnType("nvarchar(100)").IsRequired();

            builder.Property(u => u.LastName).HasColumnType("nvarchar(100)").IsRequired();

            builder.OwnsOne(u => u.Email, emailBuilder =>
            {
                emailBuilder.WithOwner();

                emailBuilder.Property(e => e.Value)
                    .HasColumnName("Email")
                    .HasColumnType($"nvarchar({EmailLengthValidator.MaxEmailLength})")
                    .IsRequired();
            });

            builder.Property(u => u.PasswordHash).HasColumnType("nvarchar(max)").IsRequired();

            builder.Property(e => e.CreatedOnUtc).HasColumnType("timestamp").IsRequired();

            builder.Property(e => e.ModifiedOnUtc).HasColumnType("timestamp").IsRequired(false);

            builder.Property(e => e.DeletedOnUtc).HasColumnType("timestamp").IsRequired(false);

            builder.Property(e => e.Deleted).HasDefaultValue(false).IsRequired();
        }
    }
}
