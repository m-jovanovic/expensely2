using System;

namespace Expensely.Infrastructure.Authentication.Entities
{
    public class UserToRoleMapping
    {
        public UserToRoleMapping(Guid userId, string roleName)
        {
            UserId = userId;
            RoleName = roleName;
        }

        public Guid UserId { get; }

        public string RoleName { get; }
    }
}