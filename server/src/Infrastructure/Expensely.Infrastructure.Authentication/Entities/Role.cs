using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Expensely.Domain;
using Expensely.Domain.Core.Primitives;
using Expensely.Infrastructure.Authentication.Abstractions;
using Expensely.Infrastructure.Authorization;

namespace Expensely.Infrastructure.Authentication.Entities
{
    /// <summary>
    /// Represents the role entity.
    /// </summary>
    internal sealed class Role
    {
        private Permission[] _permissions;

        /// <summary>
        /// Initializes a new instance of the <see cref="Role"/> class.
        /// </summary>
        /// <param name="name">The role name.</param>
        /// <param name="description">The role description.</param>
        /// <param name="permissions">The role permissions.</param>
        private Role(string name, string description, IEnumerable<Permission> permissions)
            : this()
        {
            Name = name;
            Description = description;
            _permissions = permissions.ToArray();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Role"/> class.
        /// </summary>
        /// <remarks>
        /// Required by EF Core.
        /// </remarks>
        private Role()
        {
            Name = string.Empty;
            Description = string.Empty;
            _permissions = Array.Empty<Permission>();
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the description.
        /// </summary>
        public string Description { get; private set; }

        /// <summary>
        /// Gets the permissions.
        /// </summary>
        public IReadOnlyList<Permission> Permissions => _permissions.ToArray();

        /// <summary>
        /// Updates the description and the permissions.
        /// </summary>
        /// <param name="description">The role description.</param>
        /// <param name="permissions">The role permissions.</param>
        /// <returns>The result of the update process.</returns>
        public Result Update(string description, IReadOnlyCollection<Permission> permissions)
        {
            if (!permissions.Any())
            {
                return Result.Fail(Errors.Role.AtLeastOnePermissionIsRequired);
            }

            Description = description;

            _permissions = permissions.ToArray();

            return Result.Ok();
        }

        /// <summary>
        /// Creates a new role based on the specified parameters.
        /// </summary>
        /// <param name="name">The role name.</param>
        /// <param name="description">The role description.</param>
        /// <param name="permissions">The role permissions.</param>
        /// <param name="uniquenessChecker">The role uniqueness checker.</param>
        /// <returns>The result of the creation process containing the role or an error.</returns>
        internal static async Task<Result<Role>> CreateAsync(
            string name, string description, IEnumerable<Permission> permissions, IRoleUniquenessChecker uniquenessChecker)
        {
            bool isUnique = await uniquenessChecker.IsUniqueAsync(name);

            if (!isUnique)
            {
                return Result.Fail<Role>(Errors.General.EntityAlreadyExists);
            }

            return Result.Ok(new Role(name, description, permissions));
        }
    }
}