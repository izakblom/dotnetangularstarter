using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotnetstarter.gateway.api.Application.Infrastructure.Authorization
{
    public class ClaimTypeHandler : AuthorizationHandler<ClaimTypeRequirement>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            ClaimTypeRequirement requirement)
        {

            var roles = requirement.Role.Split('#');

            var hasRole = false;

            roles.ToList().ForEach(r =>
            {
                if (context.User.HasClaim(c => c.Type == r))
                    hasRole = true;

            });




            if (hasRole)
                context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}
