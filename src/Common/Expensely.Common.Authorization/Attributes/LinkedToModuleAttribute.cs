using System;

namespace Expensely.Common.Authorization.Attributes
{
    [AttributeUsage(AttributeTargets.Field)]
    public class LinkedToModuleAttribute : Attribute
    {
        public LinkedToModuleAttribute(PaidModules paidModules)
        {
            PaidModules = paidModules;
        }

        public PaidModules PaidModules { get; }
    }
}
