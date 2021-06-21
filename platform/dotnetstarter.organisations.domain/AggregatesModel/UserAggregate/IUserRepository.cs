using Common.SeedWork;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Common.DataObjects;

namespace dotnetstarter.organisations.domain.AggregatesModel.UserAggregate
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> FindByIdAsync(int id);
        Task<User> FindByCoreUserIdRef(int coreUserIdRef);
        Task<User> FindByEmailAsync(string email);
        void Add(User user);
        void Update(User user);
        void Delete(User user);
        Task<List<DTODropdownOption>> GetCoEUsersForDropDown();
        Task<User> GetUserByIdForCommissionApi(int id);
    }
}
