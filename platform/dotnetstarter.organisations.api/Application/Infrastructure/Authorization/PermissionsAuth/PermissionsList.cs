namespace dotnetstarter.organisations.api.Application.Infrastructure.Authorization.PermissionsAuth
{
    public static class PermissionsList
    {
        //Declare constants for all permissions here to be used in attribute specifications.
        //Their constant values are translated to the corecct Permission enumeration type in PermissionRequirement.cs

        public const string MANAGE_USER_PERMISSIONS = "MANAGE_USER_PERMISSIONS";
        public const string VIEW_USERS = "VIEW_USERS";
        public const string ACCESS_ADMIN_BACKOFFICE = "ACCESS_ADMIN_BACKOFFICE";
        public const string MANAGE_USERS = "MANAGE_USERS";
        public const string MANAGE_ROLES = "MANAGE_ROLES";
    }

}
