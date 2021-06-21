using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotnetstarter.organisations.common.DataObjects;

namespace dotnetstarter.organisations.api.Application.Queries
{
    public interface IRoleQueries
    {
        Task<List<DTORole>> GetAllRolesAsync();
    }
}
