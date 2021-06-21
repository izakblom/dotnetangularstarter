using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotnetstarter.organisations.common.DataObjects
{
    public class DTORolesAndPermissions
    {
        public List<DTORole> roles { get; set; }
        public List<DTOPermission> permissions { get; set; }

        public DTORolesAndPermissions()
        {
            roles = new List<DTORole>();
            permissions = new List<DTOPermission>();
        }
    }
}
