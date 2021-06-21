using System;
using System.Collections.Generic;
using System.Text;

namespace dotnetstarter.authentication.domain.Exceptions
{
    public class AuthenticationDomainException : Exception
    {
        public AuthenticationDomainException()
        { }

        public AuthenticationDomainException(string message)
            : base(message)
        { }

        public AuthenticationDomainException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
