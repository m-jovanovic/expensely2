using System;

namespace Expensely.Common.Authorization.Attributes
{
    [AttributeUsage(AttributeTargets.Field)]
    public class LinkedToModuleAttribute : Attribute
    {
        public LinkedToModuleAttribute(PaidModule paidModule)
        {
            PaidModule = paidModule;
        }

        public PaidModule PaidModule { get; }
    }
}
