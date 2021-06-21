using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotnetstarter.organisations.common.DataObjects.Tickers;

namespace dotnetstarter.organisations.api.Application.Commands.Tickers
{
    public class GetTickerDataCommand : IRequest<ITickerResult>
    {
        public DTOTickerFilter tickerFilter;

        public GetTickerDataCommand(DTOTickerFilter _tickerFilter)
        {
            tickerFilter = _tickerFilter;
        }
    }
}
