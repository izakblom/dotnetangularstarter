using Common.Utilities.Datatable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotnetstarter.organisations.common.DataObjects.DataTable
{
    public class DTOUsersDataTableFilter : DataTableFilter
    {
        public DTOUsersFilter filter { get; set; }

        public DTOUsersDataTableFilter() : base()
        {
            filter = new DTOUsersFilter();
        }
    }
}
