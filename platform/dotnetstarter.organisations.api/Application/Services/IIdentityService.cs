using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotnetstarter.organisations.api.Application.Infrastructure.services
{
    public interface IIdentityService
    {
        string GetUserPublicKey();
        string GetUserName();
    }
}
