using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using dotnetstarter.authentication.api.Application.Commands;
using dotnetstarter.authentication.api.DTO;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace dotnetstarter.authentication.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IdentityController : ControllerBase
    {
        private IConfiguration _configuration;
        private readonly IMediator _mediator;

        public IdentityController(IConfiguration Configuration, IMediator mediator)
        {
            _configuration = Configuration;
            _mediator = mediator;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("RequestToken")]
        public async Task<IActionResult> RequestToken([FromBody] AuthenticateUserCommand request)
        {
            try
            {
                var token = await _mediator.Send(request);

                return new ObjectResult(token);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPost]
        [Route("ValidateToken")]
        public async Task<IActionResult> ValidateToken()
        {
            try
            {
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //[Authorize]
        //[HttpGet]
        //[Route("Test")]
        //public IActionResult Test()
        //{
        //    try
        //    {
        //        return Ok("Success");
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}
    }
}
