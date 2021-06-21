using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Exceptions
{
    /// <summary>
    /// Exception type for domain exceptions
    /// </summary>
    public class UserNotFoundException : Exception
    {
        public UserNotFoundException()
        { }

        public UserNotFoundException(string message)
            : base(message)
        { }

        public UserNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
