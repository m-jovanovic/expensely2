using Microsoft.EntityFrameworkCore;

namespace Expensely.Authentication
{
    public sealed class ExpenselyAuthenticationDbContext : DbContext
    {
        public ExpenselyAuthenticationDbContext(DbContextOptions options)
            : base(options)
        {
        }
    }
}
