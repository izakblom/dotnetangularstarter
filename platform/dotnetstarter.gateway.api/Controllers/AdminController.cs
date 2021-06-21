using Common.DataObjects;
using Common.Exceptions;
using dotnetstarter.gateway.api.Application.Commands.Admin;
using dotnetstarter.gateway.api.Application.Services;
using dotnetstarter.gateway.api.DataObjects.Admin;
using dotnetstarter.organisations.common.DataObjects;
using dotnetstarter.organisations.common.DataObjects.DataTable;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace dotnetstarter.gateway.api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : BaseAPIController
    {

        private IInternalAPIService _internalAPIService;
        private IMediator _mediator;

        public AdminController(IInternalAPIService internalAPIService, IMediator mediator)
        {
            _internalAPIService = internalAPIService ?? throw new ArgumentNullException(nameof(internalAPIService));

            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }


        [HttpPost]
        [Route("EnableDisableUserAccount")]
        public async Task<IActionResult> EnableDisableUserAccount(DTOEnableDisableUserAccount dtoEnableDisable)
        {
            try
            {
                var command = new EnableDisableUserAccountCommand(dtoEnableDisable);
                var result = await _mediator.Send(command);

                return Ok(result);
            }
            catch (Exception e)
            {
                throw new GeneralDomainException("Could not disable user account", e.InnerException);
            }
        }


        [HttpPost]
        [Route("AssignRevokePermission")]
        public async Task<IActionResult> AssignRevokePermission(DTOAssignRevokePermission assignRevokePermission)
        {
            try
            {
                var result = await _internalAPIService.InternalAPIPostAsync<object, DTOAssignRevokePermission>("OrganisationsAPI:AssignRevokePermission", assignRevokePermission);
                return new OkObjectResult(result);
            }
            catch (Exception e)
            {
                throw new GeneralDomainException("Could not assign-revoke permission!", e.InnerException);
            }
        }


        [HttpPost]
        [Route("AssignRevokeRole")]
        public async Task<IActionResult> AssignRevokeRole(DTOAssignRevokeRole assignRevokeRole)
        {
            try
            {
                var result = await _internalAPIService.InternalAPIPostAsync<object, DTOAssignRevokeRole>("OrganisationsAPI:AssignRevokeRole", assignRevokeRole);
                return new OkObjectResult(result);
            }
            catch (Exception e)
            {
                throw new GeneralDomainException("Could not assign-revoke role!", e.InnerException);
            }
        }


        [HttpPost]
        [Route("GetUsersByFilter")]
        public async Task<IActionResult> GetUsersByFilter([FromBody] DTOUsersDataTableFilter usersFilter)
        {
            try
            {
                var result = await _internalAPIService.InternalAPIPostAsync<DTODataTableResult<DTOUser>, DTOUsersDataTableFilter>("OrganisationsAPI:GetUsersByFilter", usersFilter);
                return new OkObjectResult(result);
            }
            catch (Exception e)
            {
                throw new GeneralDomainException("Could not retrieve users!", e.InnerException);
            }
        }



        [HttpGet]
        [Route("GetUser")]
        public async Task<IActionResult> GetUser(int id)
        {
            try
            {
                var result = await _internalAPIService.InternalAPIGetAsync<object>("OrganisationsAPI:AdminGetUser", $"?id={id}");
                return new OkObjectResult(result);
            }
            catch (Exception e)
            {
                throw new GeneralDomainException("Could not retrieve users!", e.InnerException);
            }
        }


        [HttpGet]
        [Route("GetAllRolesAndPermissions")]
        public async Task<IActionResult> GetAllRolesAndPermissions(int id)
        {
            try
            {
                var result = await _internalAPIService.InternalAPIGetAsync<object>("OrganisationsAPI:GetAllRolesAndPermissions");
                return new OkObjectResult(result);
            }
            catch (Exception e)
            {
                throw new GeneralDomainException("Could not retrieve roles and permissions!", e.InnerException);
            }
        }

        [HttpPost]
        [Route("CreateEditRole")]
        public async Task<IActionResult> CreateEditRole(DTORole dtoRole)
        {
            try
            {
                var result = await _internalAPIService.InternalAPIPostAsync<object, DTORole>("OrganisationsAPI:CreateEditRole", dtoRole);
                return new OkObjectResult(result);
            }
            catch (Exception e)
            {
                throw new GeneralDomainException("Could not assign-revoke role!", e.InnerException);
            }
        }

        [HttpPost]
        [Route("DeleteRole")]
        public async Task<IActionResult> DeleteRole(DTORole dtoRole)
        {
            try
            {
                var result = await _internalAPIService.InternalAPIPostAsync<object, DTORole>("OrganisationsAPI:DeleteRole", dtoRole);
                return new OkObjectResult(result);
            }
            catch (Exception e)
            {
                throw new GeneralDomainException("Could not assign-revoke role!", e.InnerException);
            }
        }




    }


}