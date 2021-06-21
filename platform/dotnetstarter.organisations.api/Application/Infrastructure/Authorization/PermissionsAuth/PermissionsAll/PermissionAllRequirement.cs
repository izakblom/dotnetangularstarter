using Microsoft.AspNetCore.Authorization;

namespace dotnetstarter.organisations.api.Application.Infrastructure.Authorization.PermissionsAuth
{
    internal class PermissionAllRequirement : IAuthorizationRequirement
    {
        public int[] Permissions { get; private set; }

        public PermissionAllRequirement(string[] permissions)
        {

            Permissions = PermissionsListTranslated.Translate(permissions);
        }
    }
}
