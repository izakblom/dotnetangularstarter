using System;

namespace Common.DataObjects.Ticker
{
    public class DTOTickerDateFilter
    {
        public int? DateFilterId { get; set; }
        public DateTime? To { get; set; }
        public DateTime? From { get; set; }

    }
}