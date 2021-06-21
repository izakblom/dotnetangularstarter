using Common.DataObjects;
using Common.Utilities.CustomAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotnetstarter.organisations.common.DataObjects;
using dotnetstarter.organisations.common.DataObjects.DataTable;

namespace dotnetstarter.organisations.api.Application.Queries
{
    public interface IUserQueries
    {
        Task<DTODataTableResult<DTOUser>> GetUsersByFilter(DTOUsersDataTableFilter filter);
        Task<int> GetUsersCount();
        Task<int> GetUsersTotalCountByFilter(DTOUsersDataTableFilter filter, DTODatatablesStructure queryStructure);
        Task<List<DTODropdownOption>> GetCoEUsersForDropDown();

        Task<List<DTODropdownOption>> GetCoEUsersByCostCentreIdForDropDown(int costCentreId);
    }
}
