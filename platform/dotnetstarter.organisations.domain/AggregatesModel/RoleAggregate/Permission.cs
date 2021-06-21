using Common.Exceptions;
using Common.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;

namespace dotnetstarter.organisations.domain.AggregatesModel.RoleAggregate
{
    public class Permission : Enumeration
    {
        public static Permission MANAGE_USER_PERMISSIONS = new Permission(1, "Manage User Permissions");
        public static Permission VIEW_USERS = new Permission(2, "View Users");
        public static Permission ACCESS_ADMINISTRATION_BACKOFFICE = new Permission(3, "Access Administration Backoffice");
        public static Permission MANAGE_USERS = new Permission(4, "Manage Users");
        public static Permission MANAGE_ROLES = new Permission(6, "Manage Roles");


        protected Permission() { }

        public Permission(int id, string name)
            : base(id, name)
        {
        }

        public static IEnumerable<Permission> List()
        {
            var corePermissions = new[] {
                MANAGE_USER_PERMISSIONS, VIEW_USERS, MANAGE_USERS, MANAGE_ROLES,
                ACCESS_ADMINISTRATION_BACKOFFICE,
            };
            //Join the tenant-specific permissions to the list
            if (Environment.GetEnvironmentVariable("TENANT").Equals("DEFAULT"))
            {
                corePermissions = corePermissions.Concat(dotnetstarter.organisations.domain.AggregatesModel.RoleAggregate.TenantSpecific.Tenant.TenantPermissions.List().Select(btp => new Permission(btp.Id, btp.Name))).ToArray();
            }
            return corePermissions;

        }

        public static Permission FromName(string name)
        {
            var state = List()
                .SingleOrDefault(s => String.Equals(s.Name, name, StringComparison.CurrentCultureIgnoreCase));

            if (state == null)
            {
                throw new GeneralDomainException($"Possible values for Permission: {String.Join(",", List().Select(s => s.Name))}");
            }

            return state;
        }

        public static Permission From(int id)
        {
            var state = List().SingleOrDefault(s => s.Id == id);

            if (state == null)
            {
                throw new GeneralDomainException($"Possible values for Permission: {String.Join(",", List().Select(s => s.Name))}");
            }

            return state;
        }

    }
}
