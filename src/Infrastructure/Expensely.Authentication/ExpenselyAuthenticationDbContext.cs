using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Expensely.Authentication
{
    public sealed class ExpenselyAuthenticationDbContext : IdentityDbContext
    {
        public ExpenselyAuthenticationDbContext(DbContextOptions options)
            : base(options)
        {
        }
    }
}
