using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotnetstarter.organisations.api.Application.Infrastructure.Authorization
{
    public class ClaimTypeRequirement : IAuthorizationRequirement
    {
        public string Role { get; private set; }

        public ClaimTypeRequirement(string role)
        {
            this.Role = role;
        }
    }
}
