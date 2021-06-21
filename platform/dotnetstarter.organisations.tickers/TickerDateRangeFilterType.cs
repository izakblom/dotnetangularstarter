using Common.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dotnetstarter.organisations.tickers
{
    public class TickerDateRangeFilterType : Enumeration
    {
        
        public static TickerDateRangeFilterType Today = new TickerDateRangeFilterType(1, "Today");
        public static TickerDateRangeFilterType LastSevenDays = new TickerDateRangeFilterType(2, "Last 7 Days");
        public static TickerDateRangeFilterType LastFourteenDays = new TickerDateRangeFilterType(3, "Last 14 Days");
        public static TickerDateRangeFilterType LastThirtyDays = new TickerDateRangeFilterType(4, "Last 30 Days");
        public static TickerDateRangeFilterType MonthToDate = new TickerDateRangeFilterType(5, "Month To Date");
        public static TickerDateRangeFilterType YearToDate = new TickerDateRangeFilterType(6, "Year To Date");
        public static TickerDateRangeFilterType AllTime = new TickerDateRangeFilterType(7, "All Time");
        public static TickerDateRangeFilterType Custom = new TickerDateRangeFilterType(8, "Custom Date Range");

        protected TickerDateRangeFilterType() { }

        public TickerDateRangeFilterType(int id, string name)
            : base(id, name)
        {
        }

        public static IEnumerable<TickerDateRangeFilterType> List()
        {
            return new[] { Today, LastSevenDays, LastFourteenDays, LastThirtyDays, MonthToDate, YearToDate, AllTime, Custom };
        }

        public static TickerDateRangeFilterType FromName(string name)
        {
            var state = List()
                .SingleOrDefault(s => String.Equals(s.Name, name, StringComparison.CurrentCultureIgnoreCase));

            if (state == null)
            {
                throw new Exception($"Possible values for TickerDateRangeFilterType: {String.Join(",", List().Select(s => s.Name))}");
            }

            return state;
        }

        public static TickerDateRangeFilterType From(int id)
        {
            var state = List().SingleOrDefault(s => s.Id == id);

            if (state == null)
            {
                throw new Exception($"Possible values for TickerDateRangeFilterType: {String.Join(",", List().Select(s => s.Name))}");
            }

            return state;
        }
        
    }
}
