using System;
using System.Threading.Tasks;
using Common.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using dotnetstarter.gateway.domain.AggregatesModel.UserAggregate;
using dotnetstarter.gateway.api.Application.Commands;
using dotnetstarter.gateway.api.DataObjects;
using Common.Utilities.CustomAttributes;
using dotnetstarter.gateway.api.Application.Commands.Users;
using Microsoft.AspNetCore.Http;
using dotnetstarter.organisations.common.DataObjects;
using DTOUserProfile = dotnetstarter.gateway.api.DataObjects.DTOUserProfile;

namespace dotnetstarter.gateway.api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : BaseAPIController
    {
        private readonly IMediator _mediator;


        public UsersController(
            IMediator mediator
            )
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
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
        [Route("GetUserProfileForm")]
        public async Task<IActionResult> GetUserProfileForm()
        {
            try
            {
                var command = new GetUserProfileFormCommand(CurrentUser.id, CurrentUser.email);
                var form = await _mediator.Send(command);
                return Ok(form);
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
                var command = new CreateUpdateUserCommand(dtoCreateUpdateUser, CurrentUser.id, CurrentUser.email);
                bool result = await _mediator.Send(command);
                return Ok();
            }
            catch (Exception)
            {
                throw;
            }
        }



    }
}