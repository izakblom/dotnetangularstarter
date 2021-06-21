using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace dotnetstarter.gateway.api.Controllers
{
    public class BaseAPIController : ControllerBase
    {
        /// <summary>
        /// Gets a reference to the JWT token user if one exists.
        /// </summary>
        public ClaimUser CurrentUser
        {
            get
            {
                var email = User.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress").Value;
                var id = User.Claims.FirstOrDefault(c => c.Type == "user_id").Value;

                var res = new ClaimUser { email = email, id = id };
                return res;

            }
        }

        public class ClaimUser
        {
            public string email { get; set; }
            public string id { get; set; }
        }


    }
}