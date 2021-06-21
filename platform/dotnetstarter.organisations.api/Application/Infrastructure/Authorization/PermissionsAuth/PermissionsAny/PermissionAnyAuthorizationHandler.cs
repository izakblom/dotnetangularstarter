using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Threading.Tasks;
using dotnetstarter.organisations.domain.AggregatesModel.UserAggregate;

namespace dotnetstarter.organisations.api.Application.Infrastructure.Authorization.PermissionsAuth
{
    internal class PermissionAnyAuthorizationHandler : AuthorizationHandler<PermissionAnyRequirement>
    {
        private IUserRepository _userRepository;
        private IHttpContextAccessor _httpContextAccessor;

        public PermissionAnyAuthorizationHandler(IUserRepository userRepository, IHttpContextAccessor httpContextAccessor)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        // Check whether Current User has the required permissions as specified in the requirement
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionAnyRequirement requirement)
        {
            //Check in HttpContext, since multi-trigger per request
            var user = (User)_httpContextAccessor.HttpContext.Items["User"];
            int coreUserIdRef = (int)_httpContextAccessor.HttpContext.Items["Req-User-Id"];
            if (user == null)
                user = await _userRepository.FindByCoreUserIdRef(coreUserIdRef);
            //Cache in httpContext (will only be per request, since scoped service)
            _httpContextAccessor.HttpContext.Items["User"] = user;
            if (user.UserPermissions != null)
            {
                bool atLeastOnePermission = false;
                foreach (var permission in requirement.Permissions)
                {
                    if (user.UserPermissions.Any(prm => prm.Permission.Id.Equals(permission)))
                    {
                        atLeastOnePermission = true;
                        break;
                    }
                }
                if (atLeastOnePermission)
                    context.Succeed(requirement);
            }




            //return Task.CompletedTask;
        }
    }
}
