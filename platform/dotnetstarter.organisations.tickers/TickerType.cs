using Common.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dotnetstarter.organisations.tickers
{
    public class TickerType : Enumeration
    {
        public static TickerType NUMBER_CARD = new TickerType(1, "Number Card");
        public static TickerType CHART = new TickerType(2, "Chart");

        protected TickerType() { }

        public TickerType(int id, string name)
            : base(id, name)
        {
        }

        public static IEnumerable<TickerType> List()
        {
            return new[] { NUMBER_CARD, CHART };
        }

        public static TickerType FromName(string name)
        {
            var state = List()
                .SingleOrDefault(s => String.Equals(s.Name, name, StringComparison.CurrentCultureIgnoreCase));

            if (state == null)
            {
                throw new Exception($"Possible values for TickerType: {String.Join(",", List().Select(s => s.Name))}");
            }

            return state;
        }

        public static TickerType From(int id)
        {
            var state = List().SingleOrDefault(s => s.Id == id);

            if (state == null)
            {
                throw new Exception($"Possible values for TickerType: {String.Join(",", List().Select(s => s.Name))}");
            }

            return state;
        }

    }
}
