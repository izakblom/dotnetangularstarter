using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;
using dotnetstarter.gateway.api.Application.Services;
using dotnetstarter.organisations.common.DataObjects;
using dotnetstarter.organisations.common.DataObjects.Tickers;

namespace dotnetstarter.gateway.api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TickerController : BaseAPIController
    {
        private readonly IMediator _mediator;
        private readonly IInternalAPIService _internalAPIService;
        private readonly IConfiguration _configuration;

        public TickerController(IMediator mediator, IInternalAPIService internalAPIService, IConfiguration configuration)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _internalAPIService = internalAPIService ?? throw new ArgumentNullException(nameof(internalAPIService));
        }

        [HttpGet]
        [Route("GetTickers")]
        public async Task<IActionResult> GetTickers()
        {
            try
            {
                var result = await _internalAPIService.InternalAPIGetAsync<object>("OrganisationsAPI:GetTickers");
                return new OkObjectResult(result);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet]
        [Route("GetUserDashboard")]
        public async Task<IActionResult> GetUserDashboard(string route)
        {
            try
            {
                var result = await _internalAPIService.InternalAPIGetAsync<object>("OrganisationsAPI:GetUserDashboard", $"?route={route}");
                return new OkObjectResult(result);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPost]
        [Route("GetTickerData")]
        public async Task<IActionResult> GetTickerData(DTOTickerFilter tickerFilter)
        {
            try
            {
                //Get ticker data using WithStatusFilter
                var result = await _internalAPIService.InternalAPIPostAsync<DTOTickerCountResult, DTOTickerFilter>("OrganisationsAPI:GetTickerData", tickerFilter);
                return new OkObjectResult(result);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet]
        [Route("GetTickerFilterData")]
        public async Task<IActionResult> GetTickerFilterData(int tickerId, int filterTypeId, string filter)
        {
            try
            {
                var result = await _internalAPIService.InternalAPIGetAsync<object>("OrganisationsAPI:GetTickerFilterData", $"?tickerId={tickerId}&filterTypeId={filterTypeId}&WithStatusFilter={filter}");
                return new OkObjectResult(result);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPost]
        [Route("UpdateUserDashboard")]
        public async Task<IActionResult> UpdateUserDashboard(DTOUpdateUserDashboard dto)
        {
            try
            {
                //Get ticker data using WithStatusFilter
                var result = await _internalAPIService.InternalAPIPostAsync<DTOUpdateUserDashboard>("OrganisationsAPI:UpdateUserDashboard", dto);
                return new OkObjectResult(result);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}
