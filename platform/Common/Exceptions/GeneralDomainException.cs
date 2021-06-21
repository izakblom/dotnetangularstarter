using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Exceptions
{
    /// <summary>
    /// Exception type for domain exceptions
    /// </summary>
    public class GeneralDomainException : Exception
    {
        public GeneralDomainException()
        { }

        public GeneralDomainException(string message)
            : base(message)
        { }

        public GeneralDomainException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
