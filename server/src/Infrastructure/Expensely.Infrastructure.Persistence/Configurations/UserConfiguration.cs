using Expensely.Domain.Entities;
using Expensely.Domain.ValueObjects;
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

            builder.OwnsOne(user => user.FirstName, firstNameBuilder =>
            {
                firstNameBuilder.WithOwner();

                firstNameBuilder.Property(firstName => firstName.Value)
                    .HasColumnName("first_name")
                    .HasMaxLength(FirstName.MaxLength)
                    .IsRequired();
            });

            builder.OwnsOne(user => user.LastName, lastNameBuilder =>
            {
                lastNameBuilder.WithOwner();

                lastNameBuilder.Property(lastName => lastName.Value)
                    .HasColumnName("last_name")
                    .HasMaxLength(LastName.MaxLength)
                    .IsRequired();
            });

            builder.OwnsOne(user => user.Email, emailBuilder =>
            {
                emailBuilder.WithOwner();

                emailBuilder.Property(email => email.Value)
                    .HasColumnName("email")
                    .HasMaxLength(Email.MaxLength)
                    .IsRequired();
            });

            builder.Property<string>("_passwordHash").HasField("_passwordHash").IsRequired();

            builder.Property(user => user.CreatedOnUtc).HasColumnType("timestamp").IsRequired();

            builder.Property(user => user.ModifiedOnUtc).HasColumnType("timestamp").IsRequired(false);

            builder.Property(user => user.DeletedOnUtc).HasColumnType("timestamp").IsRequired(false);

            builder.Property(user => user.Deleted).HasDefaultValue(false).IsRequired();

            builder.HasQueryFilter(user => !user.Deleted);
        }
    }
}
