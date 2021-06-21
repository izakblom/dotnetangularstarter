using Common.SeedWork;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace dotnetstarter.gateway.domain.AggregatesModel.UserAggregate
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> FindByProviderUID(string providerUID);
        Task<User> FindByIdAsync(int id);
        void Add(User user);
        void Update(User user);
        void Delete(User user);
    }
}
