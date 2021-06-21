using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotnetstarter.organisations.common.DataObjects;

namespace dotnetstarter.organisations.api.Application.Commands.Admin.Users
{
    public class AssignRevokeRoleCommand : IRequest<List<DTOPermission>>
    {
        public DTOAssignRevokeRole dtoInput { get; set; }

        public AssignRevokeRoleCommand(DTOAssignRevokeRole assignRevokeRole)
        {
            dtoInput = assignRevokeRole;
        }
    }
}
