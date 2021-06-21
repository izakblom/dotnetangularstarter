using Common.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using dotnetstarter.organisations.api.Application.Commands;
using dotnetstarter.organisations.api.Application.Commands.Auth;
using dotnetstarter.organisations.common.DataObjects;

namespace dotnetstarter.organisations.api.Controllers
{
    [Authorize(Policy = "SystemAdministrator")]
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : BaseAPIController
    {
        private readonly IMediator _mediator;

        public HomeController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpGet]
        [Route("GetPermissionedMenu")]
        public async Task<IActionResult> GetPermissionedMenu()
        {
            try
            {
                //Get user and roles, permissions
                var command = new BuildMenuCommand();
                DTOMenuItem[] result = await _mediator.Send(command);
                return new OkObjectResult(result);
            }
            catch (Exception ex)
            {
                throw new GeneralDomainException("Could not build menu!");
            }
        }

        [HttpGet]
        [Route("GetAllPermissions")]
        public async Task<IActionResult> GetAllPermissions()
        {
            try
            {
                //Get user and roles, permissions
                var command = new GetAllPermissionsCommand();
                var result = await _mediator.Send(command);
                return new OkObjectResult(result);
            }
            catch (Exception ex)
            {
                throw new GeneralDomainException("Could not retrieve all permissions!");
            }
        }

        [HttpGet]
        [Route("GetAllFeatureGuards")]
        public async Task<IActionResult> GetAllFeatureGuards()
        {
            try
            {
                //Get user and roles, permissions
                var command = new GetAllFeatureGuardsCommand();
                var result = await _mediator.Send(command);
                return new OkObjectResult(result);
            }
            catch (Exception ex)
            {
                throw new GeneralDomainException("Could not retrieve all permissions!", ex.InnerException);
            }
        }

        [HttpGet]
        [Route("ProvincesLookup")]
        public async Task<IActionResult> ProvincesLookup()
        {
            try
            {
                var res = new List<string> { "Gauteng", "Limpopo", "North-West", "Kwazulu-Natal", "Eastern Cape", "Western Cape", "Northern Cape", "Mpumalanga", "Free State" };
                return Ok(res);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
