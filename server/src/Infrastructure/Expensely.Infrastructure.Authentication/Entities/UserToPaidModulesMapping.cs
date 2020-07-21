using System;
using Expensely.Infrastructure.Authorization;

namespace Expensely.Infrastructure.Authentication.Entities
{
    public class UserToPaidModulesMapping
    {
        public UserToPaidModulesMapping(Guid userId, PaidModules paidModules)
        {
            UserId = userId;
            PaidModules = paidModules;
        }

        public Guid UserId { get; }

        public PaidModules PaidModules { get; }
    }
}
