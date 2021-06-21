using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotnetstarter.organisations.common.DataObjects
{
    public class DTOAssignRevokePermission
    {
        public int userId { get; set; }
        public int permissionId { get; set; }
    }
}
