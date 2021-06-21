using dotnetstarter.gateway.api.Application.Services;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace dotnetstarter.gateway.api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardsController : BaseAPIController
    {
        private IInternalAPIService _internalAPIService;
        private IMediator _mediator;

        public DashboardsController(IInternalAPIService internalAPIService, IMediator mediator)
        {
            _internalAPIService = internalAPIService ?? throw new ArgumentNullException(nameof(internalAPIService));

            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }


    }
}
