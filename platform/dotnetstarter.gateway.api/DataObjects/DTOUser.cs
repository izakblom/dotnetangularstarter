using Common.Utilities.CustomAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotnetstarter.organisations.common.DataObjects;

namespace dotnetstarter.gateway.api.DataObjects
{
    public class DTOUser
    {
        [Datatable("ID", "dbo.Users.Id", true, false, true)]
        public string Id { get; set; }

        [Datatable("First Name", "dbo.Users.FirstName", true, true, true)]
        public string FirstName { get; set; }

        [Datatable("Last Name", "dbo.Users.LastName", true, true, true)]
        public string LastName { get; set; }

        [Datatable("Email", "dbo.Users.Email", true, true, true)]
        public string Email { get; set; }

        public string Mobile { get; set; }

        public bool IsActive { get; set; }

        public List<DTOPermission> Permissions { get; set; }
    }
}
