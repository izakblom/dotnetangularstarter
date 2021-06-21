using System;
using System.Collections.Generic;
using System.Text;
using dotnetstarter.organisations.domain.AggregatesModel.RoleAggregate;

namespace dotnetstarter.organisations.domain.AggregatesModel.UserAggregate
{
    public class UserPermission
    {
        public User User { get; set; }
        public int UserId;

        public Permission Permission { get; set; }
        public int PermissionId;

        public UserPermission() : base()
        {

        }

        public UserPermission(int permissionId, int UserId) : this()
        {
            this.PermissionId = permissionId;
            this.UserId = UserId;
        }
    }
}
