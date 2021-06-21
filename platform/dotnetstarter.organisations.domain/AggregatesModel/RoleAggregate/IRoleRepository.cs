using Common.SeedWork;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace dotnetstarter.organisations.domain.AggregatesModel.RoleAggregate
{
    public interface IRoleRepository : IRepository<Role>
    {
        Task<Role> FindByIdAsync(int id);
        Task<List<Role>> FindAllAsync();
        void Add(Role role);
        void Update(Role role);
        void Delete(Role role);
    }
}
