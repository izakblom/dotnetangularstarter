using Common.Utilities.CustomAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotnetstarter.organisations.common.DataObjects;

namespace dotnetstarter.gateway.api.DataObjects
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

        //Below attribute commented out demonstrates usage of dropdown or radio control types with options attribute parameters
        //[DynamicFormAttribute("JWTId", "dropdown", true, false, null, null, null, new string[] { "option1", "option2" }, new string[] { "value1", "value2" })]
        public string JWTId { get; set; }

        //Below attribute commented out demonstrates usage of dropdown or radio control types with NO options attribute parameters, but a key to url specified in config from which options are to be loaded
        //[DynamicFormAttribute("JWTId", "dropdown", true, false, null, null, null, null, null, "optionsDemoUrl")]
        public bool Validated { get; set; }

        public bool Complete { get; set; }

        public string Id { get; set; }

        public List<DTOPermission> Permissions;

    }
}
