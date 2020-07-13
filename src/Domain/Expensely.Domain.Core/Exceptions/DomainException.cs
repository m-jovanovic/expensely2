using System;
using System.Runtime.Serialization;

namespace Expensely.Domain.Core.Exceptions
{
    public class DomainException : Exception
    {
        public DomainException(string errorCode)
        {
            ErrorCode = errorCode;
        }

        public string ErrorCode { get; }
    }
}
