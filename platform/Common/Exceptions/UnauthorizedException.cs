using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Exceptions
{
    /// <summary>
    /// Exception type for domain exceptions
    /// </summary>
    public class UnauthorizedException : Exception
    {
        public UnauthorizedException()
        { }

        public UnauthorizedException(string message)
            : base(message)
        { }

        public UnauthorizedException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
