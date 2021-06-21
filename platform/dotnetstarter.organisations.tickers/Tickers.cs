using System.Collections.Generic;

namespace dotnetstarter.organisations.tickers
{
    public static class Tickers
    {
        #region Users

        //public static readonly Ticker USERS_ACTIVECOUNT = new Ticker
        //{
        //    Id = 1,
        //    Title = "Users: Active Count",
        //    Type = TickerType.NUMBER_CARD,
        //    Category = TickerCategory.Users,
        //    Currency = false,
        //    Sensitive = false,
        //    Permissions = new List<domain.AggregatesModel.RoleAggregate.Permission>()
        //    {
        //        new Permission(BTPermissions.ADMIN_COE_USER.Id,BTPermissions.ADMIN_COE_USER.Name)
        //    },
        //    AvailableFilters = new List<AvailableFilter>()
        //    {
        //        new TickerDateRangeFilter("Users logged in between the following dates"),
        //    }
        //};

        //public static readonly Ticker USERS_LoginCount = new Ticker()
        //{
        //    Id = 2,
        //    Title = "Users: Login Count",
        //    Type = TickerType.NUMBER_CARD,
        //    Category = TickerCategory.Users,
        //    Currency = false,
        //    Sensitive = false,
        //    Permissions = new List<domain.AggregatesModel.RoleAggregate.Permission>()
        //    {
        //        Permission.VIEW_USERS
        //    },
        //    AvailableFilters = new List<AvailableFilter>()
        //    {
        //        new TickerDateRangeFilter("Users logged in between the following dates"),
        //    }
        //};

        #endregion Users



        public static List<Ticker> All
        {
            get
            {
                return new List<Ticker>()
                {






                };
            }
        }
    }
}