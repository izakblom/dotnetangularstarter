using Microsoft.AspNetCore.Authorization;

namespace dotnetstarter.organisations.api.Application.Infrastructure.Authorization.PermissionsAuth
{
    internal class PermissionAnyRequirement : IAuthorizationRequirement
    {
        public int[] Permissions { get; private set; }

        public PermissionAnyRequirement(string[] permissions)
        {
            Permissions = PermissionsListTranslated.Translate(permissions);
        }
    }
}
