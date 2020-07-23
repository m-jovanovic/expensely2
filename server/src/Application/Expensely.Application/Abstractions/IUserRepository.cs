using System.Threading.Tasks;

namespace Expensely.Application.Abstractions
{
    /// <summary>
    /// Represents the user repository interface.
    /// </summary>
    public interface IUserRepository
    {
        /// <summary>
        /// Checks if the specified email is unique among all existing users.
        /// </summary>
        /// <param name="email">The email to be checked.</param>
        /// <returns>True if the specified email is unique, otherwise false.</returns>
        Task<bool> IsUniqueAsync(string email);
    }
}
