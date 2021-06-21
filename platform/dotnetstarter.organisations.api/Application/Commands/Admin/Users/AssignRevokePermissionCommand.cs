using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotnetstarter.organisations.common.DataObjects;

namespace dotnetstarter.organisations.api.Application.Commands.Admin.Users
{


    public class AssignRevokePermissionCommand : IRequest<List<DTOPermission>>
    {
        public DTOAssignRevokePermission inputDTO { get; set; }

        public AssignRevokePermissionCommand(DTOAssignRevokePermission assignRevokePermission)
        {
            inputDTO = assignRevokePermission;
        }
    }
}
