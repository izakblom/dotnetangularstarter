using Common.SeedWork;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.DataObjects;
using dotnetstarter.organisations.domain.AggregatesModel.UserAggregate;

namespace dotnetstarter.organisations.infrastructure
{
    public class UserRepository : IUserRepository, IDisposable
    {
        private readonly OrganisationsContext _context;

        public UserRepository(OrganisationsContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IUnitOfWork UnitOfWork => _context;

        public void Dispose()
        {

        }

        public async Task<List<DTODropdownOption>> GetCoEUsersForDropDown()
        {
            var dropDownOptions = new List<DTODropdownOption>();
            var users = await _context.UserPermissions.Include(c => c.User).AsNoTracking()
                .Where(c => c.PermissionId == 405).ToListAsync();
            users.ForEach(u =>
            {
                dropDownOptions.Add(new DTODropdownOption()
                {
                    Key = u.UserId.ToString(),
                    Value = u.User.FirstName + " " +u.User.LastName
                });
            });
            return dropDownOptions;
        }

        public void Add(User user)
        {
            _context.Users.Add(user);
        }

        public void Delete(User user)
        {
            _context.Users.Remove(user);
        }

        public async Task<User> GetUserByIdForCommissionApi(int id)
        {

            var user = await _context.Users
                .SingleOrDefaultAsync(u => u.Id == id);

            return user;
        }

        public async Task<User> FindByIdAsync(int id)
        {
            var user = await _context.Users
                    .Include(u => u.UserPermissions)
                        .ThenInclude(up => up.Permission)
                    .SingleOrDefaultAsync(u => u.Id == id);

            return user;
        }

        public async Task<User> FindByEmailAsync(string email)
        {
            var user = await _context.Users
                    .SingleOrDefaultAsync(u => u.Email == email);

            return user;
        }


        public async Task<User> FindByCoreUserIdRef(int coreUserIdRef)
        {
            var user = await _context.Users
                .Include(u => u.UserPermissions)
                    .ThenInclude(up => up.Permission)
                .SingleOrDefaultAsync(u => u.CoreUserIdRef == coreUserIdRef);

            return user;
        }

        public void Update(User user)
        {
            _context.Users.Update(user);
        }


    }
}
