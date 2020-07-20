using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace Expensely.Authentication
{
    public sealed class ExpenselyAuthenticationDbContext : DbContext
    {
        public ExpenselyAuthenticationDbContext(DbContextOptions options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(modelBuilder);
        }
    }
}
