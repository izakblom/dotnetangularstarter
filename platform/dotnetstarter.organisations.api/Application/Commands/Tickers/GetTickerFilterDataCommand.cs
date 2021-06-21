using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotnetstarter.organisations.common.DataObjects.Tickers;
using dotnetstarter.organisations.tickers;

namespace dotnetstarter.organisations.api.Application.Commands.Tickers
{
    public class GetTickerFilterDataCommand : IRequest<List<DTOOption>>
    {
        public Ticker ticker { get; set; }
        public TickerFilterType filterType { get; set; }

        public GetTickerFilterDataCommand(Ticker _ticker, TickerFilterType _filterType)
        {
            ticker = _ticker;
            filterType = _filterType;
        }
    }
}
