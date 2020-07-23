using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace Expensely.Infrastructure.Authentication
{
    /// <summary>
    /// Represents the applications authentication database context.
    /// </summary>
    internal sealed class ExpenselyAuthenticationDbContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExpenselyAuthenticationDbContext"/> class.
        /// </summary>
        /// <param name="options">The database context options.</param>
        public ExpenselyAuthenticationDbContext(DbContextOptions options)
            : base(options)
        {
        }

        /// <inheritdoc />
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(modelBuilder);
        }
    }
}
