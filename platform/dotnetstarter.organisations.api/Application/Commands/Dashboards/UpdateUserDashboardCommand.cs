using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotnetstarter.organisations.common.DataObjects;

namespace dotnetstarter.organisations.api.Application.Commands.Dashboards
{
    public class UpdateUserDashboardCommand : IRequest<bool>
    {
        public DTOUpdateUserDashboard dtoInput { get; set; }
    }
}
