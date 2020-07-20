using System.Security.Claims;
using System.Threading.Tasks;
using Expensely.Authentication.Entities;

namespace Expensely.Authentication.Abstractions
{
    internal interface IClaimsProvider
    {
        Task<Claim[]> GetClaimsAsync(AuthenticatedUser authenticatedUser);
    }
}