using System;
using System.Threading.Tasks;
using Common.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using dotnetstarter.organisations.api.Application.Infrastructure.Authorization.PermissionsAuth;
using dotnetstarter.organisations.domain.AggregatesModel.UserAggregate;
using dotnetstarter.organisations.api.Application.Commands;
using dotnetstarter.organisations.common.DataObjects;
using Common.Utilities.CustomAttributes;
using dotnetstarter.organisations.api.Application.Commands.Users;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;

namespace dotnetstarter.organisations.api.Controllers
{
    [Authorize(Policy = "SystemAdministrator")]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : BaseAPIController
    {
        private readonly IMediator _mediator;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public UsersController(IMediator mediator, IHttpContextAccessor httpContextAccessor)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(IHttpContextAccessor));

        }


        [HttpGet]
        [Route("GetUser")]
        public async Task<IActionResult> GetUser()
        {
            try
            {
                var command = new GetUserProfileCommand(CurrentUser.id);
                var profile = await _mediator.Send(command);
                return Ok(profile);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        [Authorize]
        [Route("GetUserById")]
        public async Task<IActionResult> GetUser(int id)
        {
            try
            {
                var command = new GetUserDetailsByIdCommand(id);
                var profile = await _mediator.Send(command);
                return new OkObjectResult(profile);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        [Authorize]
        [Route("GetLoggedInUserName")]
        public async Task<IActionResult> GetLoggedInUserName()
        {
            try
            {
                User currentUser = (User)_httpContextAccessor.HttpContext.Items["User"];
                JObject result = new JObject();
                result["name"] = $"{currentUser.FirstName} {currentUser.LastName}";
                result["id"] = currentUser.Id;
                result["email"] = currentUser.Email;
                return new OkObjectResult(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        [Route("GetUserPermissions")]
        public async Task<IActionResult> GetUserPermissions()
        {
            try
            {
                var command = new GetUserPermissionsCommand();
                var permissions = await _mediator.Send(command);
                return Ok(permissions);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        /// <summary>
        ///  adds a reference to the JWT userif one does not exist
        /// </summary>
        /// <param name="dtoCreateUpdateUser"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("CreateUpdateUser")]
        public async Task<IActionResult> CreateUpdateUser([FromBody] DTOUserProfile dtoCreateUpdateUser)
        {
            try
            {
                var command = new CreateUpdateUserCommand(dtoCreateUpdateUser);
                bool result = await _mediator.Send(command);
                return Ok(result);
            }
            catch (Exception)
            {
                throw;
            }
        }



    }
}