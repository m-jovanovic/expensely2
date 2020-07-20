using System;

namespace Expensely.Authentication.Entities
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