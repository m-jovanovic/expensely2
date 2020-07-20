using System;
using Expensely.Common.Authorization;

namespace Expensely.Authentication.Entities
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
