using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotnetstarter.organisations.api.Application.Commands.Dashboards;
using dotnetstarter.organisations.api.Application.Commands.Tickers;
using dotnetstarter.organisations.common.DataObjects;
using dotnetstarter.organisations.common.DataObjects.Tickers;
using dotnetstarter.organisations.tickers;

namespace dotnetstarter.organisations.api.Controllers
{
    [Authorize(Policy = "SystemAdministrator")]
    [Route("api/[controller]")]
    [ApiController]
    public class TickerController : BaseAPIController
    {
        private readonly IMediator _mediator;

        public TickerController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpGet]
        [Route("GetTickers")]
        public async Task<IActionResult> GetTickers()
        {
            try
            {
                //Get a list of available tickers for this user
                var command = new BuildTickersCommand();
                var result = await _mediator.Send(command);
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
                var command = new GetUserDashboardCommand(route);
                var result = await _mediator.Send(command);
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
                var command = new GetTickerFilterDataCommand(Tickers.All.Where(ti => ti.Id == tickerId).FirstOrDefault(), TickerFilterType.From(filterTypeId));
                var result = await _mediator.Send(command);
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
                var command = new GetTickerDataCommand(tickerFilter);
                var result = await _mediator.Send(command);
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
                var command = new UpdateUserDashboardCommand { dtoInput = dto };
                var result = await _mediator.Send(command);
                return new OkObjectResult(result);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}
