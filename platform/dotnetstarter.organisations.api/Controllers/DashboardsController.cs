using Common.InternalApiHttp;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace dotnetstarter.organisations.api.Controllers
{
    [Authorize(Policy = "SystemAdministrator")]
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardsController : BaseAPIController
    {
        private readonly IMediator _mediator;
        private readonly IInternalAPIService _internalAPIService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DashboardsController(
            IMediator mediator, IInternalAPIService internalAPIService,
            IHttpContextAccessor httpContextAccessor)
        {

            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _internalAPIService = internalAPIService ?? throw new ArgumentNullException(nameof(internalAPIService));
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }


    }
}
