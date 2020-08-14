using Expensely.Domain.Entities;
using Expensely.Domain.Validators.Email;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Expensely.Infrastructure.Persistence.Configurations
{
    /// <summary>
    /// Contains the <see cref="User"/> entity configuration.
    /// </summary>
    internal sealed class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("User");

            builder.HasKey(u => u.Id);

            builder.Property(u => u.FirstName).HasMaxLength(100).IsRequired();

            builder.Property(u => u.LastName).HasMaxLength(100).IsRequired();

            builder.OwnsOne(u => u.Email, emailBuilder =>
            {
                emailBuilder.WithOwner();

                emailBuilder.Property(e => e.Value)
                    .HasColumnName("Email")
                    .HasMaxLength(EmailMaxLengthValidator.MaxEmailLength)
                    .IsRequired();
            });

            builder.Property(u => u.PasswordHash).IsRequired();

            builder.Property(e => e.CreatedOnUtc).HasColumnType("timestamp").IsRequired();

            builder.Property(e => e.ModifiedOnUtc).HasColumnType("timestamp").IsRequired(false);

            builder.Property(e => e.DeletedOnUtc).HasColumnType("timestamp").IsRequired(false);

            builder.Property(e => e.Deleted).HasDefaultValue(false).IsRequired();
        }
    }
}
