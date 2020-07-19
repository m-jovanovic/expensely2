using System;

namespace Expensely.Authentication.Entities
{
    public class UserToRoleMapping
    {
        public UserToRoleMapping(Guid userId, Guid roleId)
        {
            UserId = userId;
            RoleId = roleId;
        }

        public Guid UserId { get; }

        public Guid RoleId { get; }
    }
}