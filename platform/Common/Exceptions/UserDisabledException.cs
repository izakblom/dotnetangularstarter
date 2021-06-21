using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Exceptions
{
    public class UserDisabledException : Exception
    {
        public UserDisabledException()
        { }

        public UserDisabledException(string message)
            : base(message)
        { }

        public UserDisabledException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
