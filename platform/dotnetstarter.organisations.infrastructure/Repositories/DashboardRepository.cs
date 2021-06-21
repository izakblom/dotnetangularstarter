using Common.SeedWork;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dotnetstarter.organisations.domain.AggregatesModel.DashboardAggregate;

namespace dotnetstarter.organisations.infrastructure
{
    public class DashboardRepository : IDashboardRepository, IDisposable
    {
        private readonly OrganisationsContext _context;

        public DashboardRepository(OrganisationsContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IUnitOfWork UnitOfWork => _context;

        public void Dispose()
        {

        }

        public async Task<Dashboard> FindByUserIdAndRoute(int userId, string route)
        {
            var dashb = await _context.Dashboards
                .SingleOrDefaultAsync(u => u.User.Id == userId && u.GetRoute == route);

            return dashb;
        }

        public async Task<Dashboard> FindByIdAsync(int id)
        {
            var dashb = await _context.Dashboards
                .SingleOrDefaultAsync(u => u.Id == id);

            return dashb;
        }

        public void Add(Dashboard dashboard)
        {
            _context.Dashboards.Add(dashboard);
        }

        public void Update(Dashboard dashboard)
        {
            _context.Dashboards.Update(dashboard);
        }

        public void Delete(Dashboard dashboard)
        {
            _context.Dashboards.Remove(dashboard);
        }
    }
}
