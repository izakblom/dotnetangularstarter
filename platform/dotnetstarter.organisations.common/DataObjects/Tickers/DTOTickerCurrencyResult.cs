using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotnetstarter.organisations.common.DataObjects.Tickers
{
    public class DTOTickerCurrencyResult : ITickerResult
    {
        public decimal Amount { get; set; } = 0;
    }
}
