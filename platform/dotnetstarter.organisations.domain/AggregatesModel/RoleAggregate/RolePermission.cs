using System;
using System.Collections.Generic;
using System.Text;

namespace dotnetstarter.organisations.domain.AggregatesModel.RoleAggregate
{
    public class RolePermission
    {
        public Role Role { get; set; }
        public int RoleId;

        public Permission Permission { get; set; }
        public int PermissionId;

        public RolePermission() : base()
        {

        }

        public RolePermission(int permissionId, int RoleId) : this()
        {
            this.PermissionId = permissionId;
            this.RoleId = RoleId;
        }
    }
}
