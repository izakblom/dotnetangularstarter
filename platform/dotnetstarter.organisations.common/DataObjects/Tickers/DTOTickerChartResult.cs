using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotnetstarter.organisations.common.DataObjects.Tickers
{
    public class DTOTickerChartResult : ITickerResult
    {
        public List<string> labels { get; set; }
        public List<DTOtickerChartData> datasets { get; set; }

        public DTOTickerChartResult()
        {
            datasets = new List<DTOtickerChartData>();
            labels = new List<string>();
        }
    }
    
    public class DTOtickerChartData
    {
        public string title { get; set; }
        public List<object> data { get; set; }

        public DTOtickerChartData()
        {
            data = new List<object>();
        }
    }
}
