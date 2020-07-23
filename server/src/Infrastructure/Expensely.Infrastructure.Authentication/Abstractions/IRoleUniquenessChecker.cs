using System.Threading.Tasks;

namespace Expensely.Infrastructure.Authentication.Abstractions
{
    /// <summary>
    /// Represents the role uniqueness checker interface.
    /// </summary>
    internal interface IRoleUniquenessChecker
    {
        /// <summary>
        /// Checks if the role with the specified name is unique.
        /// </summary>
        /// <param name="roleName">The role name.</param>
        /// <returns>True if the role with the specified name is unique, otherwise false.</returns>
        Task<bool> IsUniqueAsync(string roleName);
    }
}