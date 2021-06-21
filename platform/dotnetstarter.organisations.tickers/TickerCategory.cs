using Common.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;

namespace dotnetstarter.organisations.tickers
{
    public class TickerCategory : Enumeration
    {
        //public static TickerCategory Users = new TickerCategory(1, "Users");
        public static TickerCategory DEFAULT = new TickerCategory(2, "Default");
        protected TickerCategory() { }

        public TickerCategory(int id, string name)
            : base(id, name)
        {
        }

        public static IEnumerable<TickerCategory> List()
        {
            return new[] { DEFAULT };
        }

        public static TickerCategory FromName(string name)
        {
            var state = List()
                .SingleOrDefault(s => String.Equals(s.Name, name, StringComparison.CurrentCultureIgnoreCase));

            if (state == null)
            {
                throw new Exception($"Possible values for TickerCategory: {String.Join(",", List().Select(s => s.Name))}");
            }

            return state;
        }

        public static TickerCategory From(int id)
        {
            var state = List().SingleOrDefault(s => s.Id == id);

            if (state == null)
            {
                throw new Exception($"Possible values for TickerCategory: {String.Join(",", List().Select(s => s.Name))}");
            }

            return state;
        }

        public static List<Ticker> TickersFromCategory(string name)
        {
            var cat = List()
                .SingleOrDefault(s => String.Equals(s.Name, name, StringComparison.CurrentCultureIgnoreCase));

            if (cat == null)
            {
                throw new Exception($"Possible values for TickerCategory: {String.Join(",", List().Select(s => s.Name))}");
            }

            return Tickers.All.Where(t => t.Category == cat).ToList();

        }

    }
}
