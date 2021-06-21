using Common.Exceptions;
using dotnetstarter.gateway.api.Application.Services;
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
    public class HomeController : BaseAPIController
    {
        private readonly IMediator _mediator;
        private readonly IInternalAPIService _internalAPIService;


        public HomeController(IMediator mediator, IInternalAPIService internalAPIService)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _internalAPIService = internalAPIService ?? throw new ArgumentNullException(nameof(internalAPIService));
        }

        [HttpGet]
        [Route("GetPermissionedMenu")]
        public async Task<IActionResult> GetPermissionedMenu()
        {
            try
            {
                //Get user and roles, permissions
                var result = await _internalAPIService.InternalAPIGetAsync<object>("OrganisationsAPI:GetPermissionedMenu");
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
                var result = await _internalAPIService.InternalAPIGetAsync<object>("OrganisationsAPI:GetAllPermissions");
                return new OkObjectResult(result);
            }
            catch (Exception ex)
            {
                throw new GeneralDomainException("Could not get permissions!");
            }
        }

        [HttpGet]
        [Route("GetAllFeatureGuards")]
        public async Task<IActionResult> GetAllFeatureGuards()
        {
            try
            {
                //Get user and roles, permissions
                var result = await _internalAPIService.InternalAPIGetAsync<object>("OrganisationsAPI:GetAllFeatureGuards");
                return new OkObjectResult(result);
            }
            catch (Exception ex)
            {
                throw new GeneralDomainException("Could not get feature guards!");
            }
        }


    }
}
