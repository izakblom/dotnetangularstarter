using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotnetstarter.organisations.common.DataObjects.Dashboards
{
    public class DTODashboardResult
    {
        public bool exists { get; set; } = false;
        public string route { get; set; }
        public string serializedDashboard { get; set; }

    }
}
