using Common.Utilities.CustomAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotnetstarter.organisations.common.DataObjects
{
    public class DTORole
    {
        [DatatableAttribute("Id", "dbo.Roles.Id")]
        public int Id { get; set; }

        [DatatableAttribute("Id", "dbo.Roles.Name")]
        public string Name { get; set; }

        [DatatableAttribute("Id", "dbo.Roles.Description")]
        public string Description { get; set; }


        public List<DTOPermission> Permissions { get; set; }

        public DTORole()
        {
            Permissions = new List<DTOPermission>();
        }
    }
}
