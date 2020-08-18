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
            builder.ToTable("users");

            builder.HasKey(user => user.Id);

            builder.Property(user => user.FirstName).HasMaxLength(100).IsRequired();

            builder.Property(user => user.LastName).HasMaxLength(100).IsRequired();

            builder.OwnsOne(user => user.Email, emailBuilder =>
            {
                emailBuilder.WithOwner();

                emailBuilder.Property(email => email.Value)
                    .HasColumnName("email")
                    .HasMaxLength(EmailMaxLengthValidator.MaxEmailLength)
                    .IsRequired();
            });

            builder.Property(user => user.PasswordHash).IsRequired();

            builder.Property(user => user.CreatedOnUtc).HasColumnType("timestamp").IsRequired();

            builder.Property(user => user.ModifiedOnUtc).HasColumnType("timestamp").IsRequired(false);

            builder.Property(user => user.DeletedOnUtc).HasColumnType("timestamp").IsRequired(false);

            builder.Property(user => user.Deleted).HasDefaultValue(false).IsRequired();

            builder.HasQueryFilter(user => !user.Deleted);
        }
    }
}
