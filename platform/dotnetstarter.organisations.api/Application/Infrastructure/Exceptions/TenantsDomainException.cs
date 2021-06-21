using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotnetstarter.organisations.api.Application.Infrastructure.Exceptions
{
    public class TenantsDomainException : Exception
    {
        public TenantsDomainException()
        { }

        public TenantsDomainException(string message)
            : base(message)
        { }

        public TenantsDomainException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
