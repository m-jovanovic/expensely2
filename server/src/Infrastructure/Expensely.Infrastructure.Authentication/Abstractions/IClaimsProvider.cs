using System.Security.Claims;
using System.Threading.Tasks;
using Expensely.Infrastructure.Authentication.Entities;

namespace Expensely.Infrastructure.Authentication.Abstractions
{
    internal interface IClaimsProvider
    {
        Task<Claim[]> GetClaimsAsync(AuthenticatedUser authenticatedUser);
    }
}