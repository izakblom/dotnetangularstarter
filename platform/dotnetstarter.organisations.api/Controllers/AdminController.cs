using Common.Exceptions;
using Common.InternalApiHttp;
using dotnetstarter.organisations.api.Application.Commands.Admin.Roles;
using dotnetstarter.organisations.api.Application.Commands.Admin.Users;
using dotnetstarter.organisations.api.Application.Infrastructure.Authorization.PermissionsAuth;
using dotnetstarter.organisations.api.Application.Queries;
using dotnetstarter.organisations.common.DataObjects;
using dotnetstarter.organisations.common.DataObjects.Admin;
using dotnetstarter.organisations.common.DataObjects.DataTable;
using dotnetstarter.organisations.domain.AggregatesModel.UserAggregate;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace dotnetstarter.organisations.api.Controllers
{
    [Authorize(Policy = "SystemAdministrator")]
    [PermissionAllAuthorize(new string[] { PermissionsList.ACCESS_ADMIN_BACKOFFICE })]
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : BaseAPIController
    {
        private readonly IUserQueries _userQueries;
        private readonly IUserRepository _userRepository;
        private readonly IMediator _mediator;
        private readonly IInternalAPIService _internalAPIService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AdminController(
            IUserQueries userQueries, IUserRepository userRepository, IMediator mediator,
            IInternalAPIService internalAPIService,
             IHttpContextAccessor httpContextAccessor)
        {
            _userQueries = userQueries ?? throw new ArgumentNullException(nameof(userQueries));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _internalAPIService = internalAPIService ?? throw new ArgumentNullException(nameof(internalAPIService));
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        [PermissionAllAuthorize(new string[] { PermissionsList.MANAGE_USERS, PermissionsList.VIEW_USERS })]
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

        [PermissionAllAuthorize(new string[] { PermissionsList.MANAGE_USER_PERMISSIONS, PermissionsList.VIEW_USERS })]
        [HttpPost]
        [Route("AssignRevokePermission")]
        public async Task<IActionResult> AssignRevokePermission(DTOAssignRevokePermission assignRevokePermission)
        {
            try
            {
                var command = new AssignRevokePermissionCommand(assignRevokePermission);
                var result = await _mediator.Send(command);

                return Ok(result);
            }
            catch (Exception e)
            {
                throw new GeneralDomainException("Could not assign-revoke permission!", e.InnerException);
            }
        }

        [PermissionAllAuthorize(new string[] { PermissionsList.MANAGE_USER_PERMISSIONS, PermissionsList.VIEW_USERS })]
        [HttpPost]
        [Route("AssignRevokeRole")]
        public async Task<IActionResult> AssignRevokeRole(DTOAssignRevokeRole assignRevokeRole)
        {
            try
            {
                var command = new AssignRevokeRoleCommand(assignRevokeRole);
                var result = await _mediator.Send(command);

                return Ok(result);
            }
            catch (Exception e)
            {
                throw new GeneralDomainException("Could not assign-revoke role!", e.InnerException);
            }
        }

        [PermissionAllAuthorize(new string[] { PermissionsList.VIEW_USERS })]
        [HttpPost]
        [Route("GetUsersByFilter")]
        public async Task<IActionResult> GetUsersByFilter([FromBody] DTOUsersDataTableFilter usersFilter)
        {
            try
            {
                var res = await _userQueries.GetUsersByFilter(usersFilter);
                return Ok(res);
            }
            catch (Exception e)
            {
                throw new GeneralDomainException("Could not retrieve users!", e.InnerException);
            }
        }


        [PermissionAllAuthorize(new string[] { PermissionsList.VIEW_USERS })]
        [HttpGet]
        [Route("GetUser")]
        public async Task<IActionResult> GetUser(int id)
        {
            try
            {
                var command = new GetUserWithPermissionsCommand(id);
                var result = await _mediator.Send(command);
                return Ok(result);
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
                var command = new GetAllRolesAndPermissionsCommand();
                var result = await _mediator.Send(command);
                return Ok(result);
            }
            catch (Exception e)
            {
                throw new GeneralDomainException("Could not retrieve users!", e.InnerException);
            }
        }

        [PermissionAllAuthorize(new string[] { PermissionsList.MANAGE_ROLES })]
        [HttpPost]
        [Route("CreateEditRole")]
        public async Task<IActionResult> CreateEditRole(DTORole dtoRole)
        {
            try
            {
                var command = new CreateEditRoleCommand(dtoRole);
                var result = await _mediator.Send(command);

                return Ok(result);
            }
            catch (Exception e)
            {
                throw new GeneralDomainException("Could not create-edit role!", e.InnerException);
            }
        }

        [PermissionAllAuthorize(new string[] { PermissionsList.MANAGE_ROLES })]
        [HttpPost]
        [Route("DeleteRole")]
        public async Task<IActionResult> DeleteRole(DTORole dtoRole)
        {
            try
            {
                var command = new DeleteRoleCommand(dtoRole);
                var result = await _mediator.Send(command);

                return Ok(result);
            }
            catch (Exception e)
            {
                throw new GeneralDomainException("Could not delete role!", e.InnerException);
            }
        }


    }


}