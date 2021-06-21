using dotnetstarter.authentication.domain.AggregatesModel.IdentityAggregate;
using dotnetstarter.authentication.domain.SeedWork;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotnetstarter.authentication.infrastructure.Repositories
{
    public class IdentityRepository : IIdentityRepository
    {
        private readonly IdentityContext _context;
        public IdentityRepository(IdentityContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IUnitOfWork UnitOfWork => _context;

        public Identity Add(Identity user)
        {
            _context.Users.Add(user);

            return user;
        }

        public async Task<Identity> FindByIdAsync(Guid id)
        {
            var user = await _context
                                .Users
                                .SingleOrDefaultAsync(u => u.Id == id);

            return user;
        }

        public async Task<Identity> FindByUsernameAsync(string username)
        {
            var user = await _context
                                .Users
                                .Include(u => u.Claims)
                                    .ThenInclude(c => c.ClaimType)
                                .SingleOrDefaultAsync(u => u.Username == username);

            return user;
        }

        public void Update(Identity user)
        {
            _context.Users.Update(user);
        }
    }
}
