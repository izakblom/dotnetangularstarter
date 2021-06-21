using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotnetstarter.gateway.api.Controllers;

namespace dotnetstarter.root.api.Controllers
{
    [Route("/")]
    [ApiController]
    public class HealthController : BaseAPIController
    {

        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return Ok();
        }
    }
}
