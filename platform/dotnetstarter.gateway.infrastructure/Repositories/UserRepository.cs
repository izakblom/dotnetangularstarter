using Common.SeedWork;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dotnetstarter.gateway.domain.AggregatesModel.UserAggregate;

namespace dotnetstarter.gateway.infrastructure
{
    public class UserRepository : IUserRepository, IDisposable
    {
        private readonly PortalContext _context;

        public UserRepository(PortalContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IUnitOfWork UnitOfWork => _context;

        public void Dispose()
        {

        }

        public void Add(User user)
        {
            _context.Users.Add(user);
        }

        public void Delete(User user) 
        {
            _context.Users.Remove(user);
        }

        public async Task<User> FindByIdAsync(int id)
        {
            var user = await _context.Users
                    .SingleOrDefaultAsync(u => u.Id == id);

            return user;
        }


        public async Task<User> FindByProviderUID(string providerUID)
        {
            var user = await _context.Users
                .SingleOrDefaultAsync(u => u.JWTId == providerUID);

            return user;

        }

        public void Update(User user)
        {
            _context.Users.Update(user);
        }
    }
}
