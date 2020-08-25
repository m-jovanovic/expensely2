using System;
using System.Collections.Generic;
using System.Linq;
using Expensely.Domain.Core.Exceptions;

namespace Expensely.Domain.Authorization
{
    /// <summary>
    /// Represents the role entity.
    /// </summary>
    public sealed class Role
    {
        private Permission[] _permissions;

        /// <summary>
        /// Initializes a new instance of the <see cref="Role"/> class.
        /// </summary>
        /// <param name="name">The role name.</param>
        /// <param name="description">The role description.</param>
        /// <param name="permissions">The role permissions.</param>
        public Role(string name, string description, IEnumerable<Permission> permissions)
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
        public void Update(string description, IReadOnlyCollection<Permission> permissions)
        {
            if (!permissions.Any())
            {
                throw new DomainException(Errors.Role.AtLeastOnePermissionIsRequired);
            }

            Description = description;

            _permissions = permissions.ToArray();
        }
    }
}