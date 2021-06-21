using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotnetstarter.organisations.common.DataObjects.Tickers
{
    public class DTOTickerFilter
    {
        public string textFilter { get; set; }
        public List<DTOCollectionFilter> collectionFilters { get; set; }

        public int selectedDateRangeFilter { get; set; }
        public DateTime? from { get; set; }
        public DateTime? to { get; set; }
        public int templateId { get; set; }
        public int selectedTicker { get; set; }                         // see Ticker.Id

        public DTOTickerFilter() { collectionFilters = new List<DTOCollectionFilter>(); }
    }

    public class DTOCollectionFilter
    {
        public DTOFilterType type { get; set; }
        public List<DTOOption> items { get; set; }

    }

    public class DTOFilterType
    {
        public int id { get; set; }
        public string name { get; set; }

    }

    public class DTOOption
    {
        public int id { get; set; }
        public string value { get; set; }

    }

}
