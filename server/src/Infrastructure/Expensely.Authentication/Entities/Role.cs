using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Expensely.Authentication.Abstractions;
using Expensely.Common.Authorization;
using Expensely.Domain;
using Expensely.Domain.Core.Primitives;

namespace Expensely.Authentication.Entities
{
    public class Role
    {
        private Permission[] _permissions;

        private Role(string name, string description, IEnumerable<Permission> permissions)
        {
            Name = name;
            Description = description;
            _permissions = permissions.ToArray();
        }

        public string Name { get; }

        public string Description { get; private set; }

        public IReadOnlyList<Permission> Permissions => _permissions.ToArray();

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

        internal static async Task<Result<Role>> CreateAsync(
            string name, string description, IEnumerable<Permission> permissions, IRoleUniquenessChecker uniquenessChecker)
        {
            bool isUnique = await uniquenessChecker.IsUniqueAsync(name);

            if (!isUnique)
            {
                return Result.Fail<Role>(Errors.General.AlreadyExists);
            }

            return Result.Ok(new Role(name, description, permissions));
        }
    }
}