using System;
using System.Collections.Generic;
using System.ComponentModel;
using dotnetstarter.organisations.domain.AggregatesModel.RoleAggregate;
using System.Linq;

namespace dotnetstarter.organisations.tickers
{
    public class Ticker
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public bool Sensitive { get; set; } = false;
        public TickerCategory Category { get; set; }
        public TickerType Type { get; set; } = TickerType.NUMBER_CARD;
        public bool Currency { get; set; } = false;
        public List<Permission> Permissions { get; set; }
        public List<AvailableFilter> AvailableFilters { get; set; }

        public Ticker()
        {
            Permissions = new List<Permission>();
            AvailableFilters = new List<AvailableFilter>();
        }
    }

    public class TickerDateRangeFilter : AvailableFilter
    {
        public TickerDateRangeFilter(string descr) : base()
        {
            Type = TickerFilterType.DATERANGE;

            Options = new DataOptions()
            {
                Lookup = true,
                RemoteFetch = false
            };

            TickerDateRangeFilterType.List().ToList().ForEach((f) => { Options.Options.Add(new DataOptions.Option(f.Id, f.Name)); });
            Description = descr;
        }
    }

    public class TickerCollectionFilter : AvailableFilter
    {
       
        public TickerCollectionFilter(TickerFilterType _type, string descr) : base()
        {
            Type = _type;

            Options = new DataOptions()
            {
                Lookup = true,
                RemoteFetch = true
            };

            Description = descr;

        }
    }

    public class TickerTextFilter : AvailableFilter
    {
        public TickerTextFilter(string descr) : base()
        {
            Type = TickerFilterType.TEXTSEARCH;

            Options = new DataOptions()
            {
                Lookup = false,
                RemoteFetch = false
            };

            Description = descr;
        }

    }

    public abstract class AvailableFilter
    {
        public string Description { get; set; }         // helper text for user to know what the WithStatusFilter does.
        public TickerFilterType Type { get; set; }
        public DataOptions Options { get; set; }
    }

    public class DataOptions
    {
        public bool Lookup { get; set; } = false;
        public bool RemoteFetch { get; set; } = false;
        public List<Option> Options { get; set; } = null;

        public class Option
        {
            public object Id { get; set; }
            public object Value { get; set; }

            public Option(object id, object value)
            {
                Id = id;
                Value = value;
            }
        }

        public DataOptions() { Options = new List<Option>(); }
    }

}
