using System.Collections.Generic;

namespace dotnetstarter.organisations.domain.AggregatesModel.RoleAggregate
{
    public static class FeaturePermissions
    {
        public static List<FeatureGuard> ALL
        {
            get
            {
                return new List<FeatureGuard>() {
                    ADMIN_SECTION,
                    ADMIN_DASHBOARD,
                    USERS_LIST,
                    USER_DETAILS,
                    CONFIGURE_ROLE,
                    DISABLE_USER,
                    SET_USER_PERMISSIONS,

                };
            }
        }

        public static FeatureGuard ADMIN_SECTION
        {
            get
            {
                return new FeatureGuard()
                {
                    Name = "ADMIN_SECTION",
                    UrlRegex = ".*?(\\/admin)", //matches '*/admin*'
                    PermissionsAll = new List<Permission>() {
                        Permission.ACCESS_ADMINISTRATION_BACKOFFICE,
                    }
                };
            }
        }

        public static FeatureGuard ADMIN_DASHBOARD
        {
            get
            {
                return new FeatureGuard()
                {
                    Name = "ADMIN_DASHBOARD",
                    UrlRegex = "^.*?(\\/admin)(\\/dashboard)$", //matches '*/admin/dashboard'
                    PermissionsAll = new List<Permission>() {
                        Permission.ACCESS_ADMINISTRATION_BACKOFFICE,
                    }
                };
            }
        }

        public static FeatureGuard USERS_LIST
        {
            get
            {
                return new FeatureGuard()
                {
                    Name = "USERS_LIST",
                    UrlRegex = ".*?(\\/admin)(\\/users)$", //matches '*/admin/users'
                    PermissionsAll = new List<Permission>() {
                        Permission.ACCESS_ADMINISTRATION_BACKOFFICE,
                        Permission.VIEW_USERS
                    }
                };
            }
        }

        public static FeatureGuard USER_DETAILS
        {
            get
            {
                return new FeatureGuard()
                {
                    Name = "USER_DETAILS",
                    UrlRegex = ".*?(\\/admin)(\\/users\\/).*", //matches '*/admin/users/*'
                    PermissionsAll = new List<Permission>() {
                        Permission.ACCESS_ADMINISTRATION_BACKOFFICE,
                        Permission.VIEW_USERS
                    }
                };
            }
        }

        public static FeatureGuard CONFIGURE_ROLE
        {
            get
            {
                return new FeatureGuard()
                {
                    Name = "CONFIGURE_ROLE",
                    UrlRegex = ".*?(\\/admin)(\\/roles).*", //matches '*/admin/roles*'
                    PermissionsAll = new List<Permission>() {
                        Permission.ACCESS_ADMINISTRATION_BACKOFFICE,
                        Permission.MANAGE_ROLES
                    }
                };
            }
        }



        public static FeatureGuard DISABLE_USER
        {
            get
            {
                return new FeatureGuard()
                {
                    Name = "DISABLE_USER",
                    PermissionsAll = new List<Permission>() {
                        Permission.ACCESS_ADMINISTRATION_BACKOFFICE,
                        Permission.VIEW_USERS,
                        Permission.MANAGE_USERS
                    }
                };
            }
        }

        public static FeatureGuard SET_USER_PERMISSIONS
        {
            get
            {
                return new FeatureGuard()
                {
                    Name = "SET_USER_PERMISSIONS",
                    PermissionsAll = new List<Permission>() {
                        Permission.ACCESS_ADMINISTRATION_BACKOFFICE,
                        Permission.VIEW_USERS,
                        Permission.MANAGE_USER_PERMISSIONS
                    }
                };
            }
        }






    }

    public class FeatureGuard
    {
        public string Name { get; set; }
        /// <summary>
        /// Use https://regex101.com/ to verify regular expressions
        /// </summary>
        public string UrlRegex { get; set; }

        /// <summary>
        /// Define permissions to be checked in AND manner, i.e. all permissions should be allocated to the user
        /// </summary>
        public List<Permission> PermissionsAll { get; set; }

        /// <summary>
        /// Define permissions to be checked in OR manner, i.e. at least one permission in the list should be allocated to the user
        /// </summary>
        public List<Permission> PermissionsAny { get; set; }

    }
}
