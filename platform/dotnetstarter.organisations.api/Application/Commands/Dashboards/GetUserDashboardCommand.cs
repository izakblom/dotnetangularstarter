using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotnetstarter.organisations.common.DataObjects.Dashboards;

namespace dotnetstarter.organisations.api.Application.Commands.Dashboards
{
    public class GetUserDashboardCommand : IRequest<DTODashboardResult>
    {
        public readonly string route;

        public GetUserDashboardCommand(string _route)
        {
            route = _route;
        }
    }
}
