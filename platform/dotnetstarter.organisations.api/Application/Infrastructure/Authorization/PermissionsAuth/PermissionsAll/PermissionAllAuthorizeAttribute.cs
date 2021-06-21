using Microsoft.AspNetCore.Authorization;
using System;

namespace dotnetstarter.organisations.api.Application.Infrastructure.Authorization.PermissionsAuth
{
    public class PermissionAllAuthorizeAttribute : AuthorizeAttribute
    {
        const string POLICY_PREFIX = "PermissionsAll";

        /// <summary>
        /// Provide an array of permission name constants from PermissionsList.cs to be checked
        /// as assigned to the current user as part of the authorization requirements
        /// </summary>
        /// <param name="permissions"></param>
        public PermissionAllAuthorizeAttribute(string[] permissions) => Permissions = permissions;

        public string[] Permissions
        {
            get
            {
                string sub = Policy.Substring(POLICY_PREFIX.Length);
                if (!String.IsNullOrEmpty(sub))
                {
                    return sub.Split("#");
                }
                return new string[] { };
            }
            set
            {
                Policy = $"{POLICY_PREFIX}{string.Join("#", value)}";
            }
        }
    }
}
