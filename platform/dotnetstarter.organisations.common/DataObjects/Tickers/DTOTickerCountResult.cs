using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotnetstarter.organisations.common.DataObjects.Tickers
{
    public class DTOTickerCountResult: ITickerResult
    {
        public int Count { get; set; } = 0;
    }
}
