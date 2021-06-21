using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Common.SeedWork;
using Microsoft.EntityFrameworkCore;
using dotnetstarter.organisations.domain.AggregatesModel.RoleAggregate;

namespace dotnetstarter.organisations.infrastructure.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly OrganisationsContext _context;
        public IUnitOfWork UnitOfWork => _context;

        public RoleRepository(OrganisationsContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public void Add(Role role)
        {
            _context.Roles.Add(role);
        }

        public void Delete(Role role)
        {
            _context.Roles.Remove(role);
        }

        public async Task<Role> FindByIdAsync(int id)
        {
            var role = await _context.Roles
                .Include(r => r.RolePermissions)
                    .ThenInclude(rp => rp.Permission)
                .SingleOrDefaultAsync(r => r.Id == id);

            return role;
        }

        public async Task<List<Role>> FindAllAsync()
        {
            var roles = await _context.Roles
                .Include(r => r.RolePermissions)
                    .ThenInclude(rp => rp.Permission)
                .ToListAsync();
            return roles;
        }

        public void Update(Role role)
        {
            _context.Roles.Update(role);
        }
    }
}
