using Common.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;

namespace dotnetstarter.organisations.tickers
{
    public class TickerFilterType : Enumeration
    {
        public static TickerFilterType DEFAULT = new TickerFilterType(1, "DEFAULT");

        public static TickerFilterType DATERANGE = new TickerFilterType(4, "DATE RANGE");
        public static TickerFilterType TEXTSEARCH = new TickerFilterType(5, "TEXT SEARCH");

        protected TickerFilterType() { }

        public TickerFilterType(int id, string name)
            : base(id, name)
        {
        }

        public static IEnumerable<TickerFilterType> List()
        {
            return new[] {  DEFAULT, DATERANGE, TEXTSEARCH,
            };
        }

        public static TickerFilterType FromName(string name)
        {
            var state = List()
                .SingleOrDefault(s => String.Equals(s.Name, name, StringComparison.CurrentCultureIgnoreCase));

            if (state == null)
            {
                throw new Exception($"Possible values for TickerFilterType: {String.Join(",", List().Select(s => s.Name))}");
            }

            return state;
        }

        public static TickerFilterType From(int id)
        {
            var state = List().SingleOrDefault(s => s.Id == id);

            if (state == null)
            {
                throw new Exception($"Possible values for TickerFilterType: {String.Join(",", List().Select(s => s.Name))}");
            }

            return state;
        }

    }
}
