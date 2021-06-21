using dotnetstarter.organisations.tickers;
using MediatR;
using System.Collections.Generic;

namespace dotnetstarter.organisations.api.Application.Commands.Tickers
{
    public class BuildTickersCommand : IRequest<Dictionary<TickerCategory, List<Ticker>>>
    {

        public BuildTickersCommand()
        {
        }
    }
}
