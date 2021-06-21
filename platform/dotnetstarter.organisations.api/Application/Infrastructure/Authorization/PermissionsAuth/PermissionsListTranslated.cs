using dotnetstarter.organisations.domain.AggregatesModel.RoleAggregate;
using System;
using System.Collections.Generic;

namespace dotnetstarter.organisations.api.Application.Infrastructure.Authorization.PermissionsAuth
{
    public static class PermissionsListTranslated
    {
        public static int[] Translate(string[] permissions)
        {
            //Translate the provided permission string constants to the correct Permission Enumeration type's name
            List<int> translated = new List<int>();
            foreach (var permission in permissions)
            {
                //TODO add more clauses here for new permissions
                if (permission.Equals(PermissionsList.MANAGE_USER_PERMISSIONS))
                    translated.Add(Permission.MANAGE_USER_PERMISSIONS.Id);
                else if (permission.Equals(PermissionsList.VIEW_USERS))
                    translated.Add(Permission.VIEW_USERS.Id);
                else if (permission.Equals(PermissionsList.ACCESS_ADMIN_BACKOFFICE))
                    translated.Add(Permission.ACCESS_ADMINISTRATION_BACKOFFICE.Id);
                else if (permission.Equals(PermissionsList.MANAGE_USERS))
                    translated.Add(Permission.MANAGE_USERS.Id);
                else if (permission.Equals(PermissionsList.MANAGE_ROLES))
                    translated.Add(Permission.MANAGE_ROLES.Id);
                else
                {
                    //Check in tenant specific list
                    if (Environment.GetEnvironmentVariable("TENANT").Equals("DEFAULT"))
                    {
                        if (permission.Equals(dotnetstarter.organisations.api.Application.Infrastructure.Authorization.PermissionsAuth.TenantSpecific.Tenant.TenantPermissionsList.DEFAULT))
                            translated.Add(dotnetstarter.organisations.domain.AggregatesModel.RoleAggregate.TenantSpecific.Tenant.TenantPermissions.DEFAULT.Id);

                    }
                }
            }
            return translated.ToArray();
        }
    }
}
