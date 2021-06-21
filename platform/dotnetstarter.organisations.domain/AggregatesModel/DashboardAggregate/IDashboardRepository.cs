using Common.SeedWork;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace dotnetstarter.organisations.domain.AggregatesModel.DashboardAggregate
{
    public interface IDashboardRepository : IRepository<Dashboard>
    {
        Task<Dashboard> FindByUserIdAndRoute(int userId, string route);
        Task<Dashboard> FindByIdAsync(int id);
        void Add(Dashboard dashboard);
        void Update(Dashboard dashboard);
        void Delete(Dashboard dashboard);
    }
}
