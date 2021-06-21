using dotnetstarter.authentication.domain.SeedWork;
using System;
using System.Threading.Tasks;

namespace dotnetstarter.authentication.domain.AggregatesModel.IdentityAggregate
{
    public interface IIdentityRepository : IRepository<Identity>
    {
        Identity Add(Identity user);
        void Update(Identity user);
        Task<Identity> FindByIdAsync(Guid id);
        Task<Identity> FindByUsernameAsync(string username);
    }
}
