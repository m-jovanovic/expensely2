using System.Security.Claims;
using System.Threading.Tasks;

namespace Expensely.Authentication.Abstractions
{
    internal interface IClaimsProvider
    {
        Task<Claim[]> GetClaimsAsync(dynamic user);
    }
}