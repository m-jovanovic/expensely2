using System.Threading.Tasks;

namespace Expensely.Authentication.Abstractions
{
    internal interface IRoleUniquenessChecker
    {
        Task<bool> IsUniqueAsync(string roleName);
    }
}