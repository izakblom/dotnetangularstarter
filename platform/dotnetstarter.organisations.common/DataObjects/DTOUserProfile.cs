using Common.Utilities.CustomAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotnetstarter.organisations.common.DataObjects
{
    public class DTOUserProfile
    {
        [DynamicFormAttribute("First Name", "textbox", true)]
        public string FirstName { get; set; }

        [DynamicFormAttribute("Last Name", "textbox", true)]
        public string LastName { get; set; }

        [DynamicFormAttribute("Email", "textbox", true, true, true, "^[a-zA-Z0-9.!#$%&’*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\\.[a-zA-Z0-9-]+)*$", "Please enter a valid email address", "email")]
        public string Email { get; set; }

        [DynamicFormAttribute("Mobile Number", "textbox", true, false, true, "^((\\+27|27)|0)(\\d{2})(\\d{7})$", "Please enter a valid South African mobile number", "tel")]
        public string MobileNumber { get; set; }

        [DynamicFormAttribute("ID Number", "textbox", true, false, true, "^([0-9]{2})(0[1-9]|1[0-2])(0[1-9]|1[0-9]|2[0-9]|3[0-1])(\\d{7})$", "Please enter a valid South African identity number", "tel")]
        public string IDNumber { get; set; }

        public int CoreUserIdRef { get; set; }

        public string Id { get; set; }

        public DTOPermission[] Permissions;

    }
}
