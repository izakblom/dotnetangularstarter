using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Common.DataObjects.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using dotnetstarter.organisations.common.DataObjects;

namespace dotnetstarter.organisations.api.Controllers
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

        protected async Task<DTOToken> GetAPIAccessToken(IConfiguration configuration)
        {
            var token = new DTOToken();

            //authenticate backend request.
            using (HttpClient client = new HttpClient())
            {
                var tokenReq = new DTOTokenRequest();
                tokenReq.Username = configuration[$"{Environment.GetEnvironmentVariable("TENANT")}:Identity:Username"];
                tokenReq.Password = configuration[$"{Environment.GetEnvironmentVariable("TENANT")}:Identity:Password"];

                //get the auth token.
                var resp = await client.PostAsJsonAsync<DTOTokenRequest>($"{configuration[$"{Environment.GetEnvironmentVariable("TENANT")}:Identity:RequestTokenAddress"]}", tokenReq);
                var respstr = await resp.Content.ReadAsStringAsync();

                //got the authentication token.
                token = JsonConvert.DeserializeObject<DTOToken>(respstr);

                return token;
            }
        }
    }
}