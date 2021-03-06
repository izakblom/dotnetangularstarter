using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotnetstarter.organisations.common.DataObjects;

namespace dotnetstarter.organisations.api.Application.Commands.Admin.Roles
{
    public class CreateEditRoleCommand : IRequest<bool>
    {
        public DTORole dtoRole { get; set; }

        public CreateEditRoleCommand(DTORole input)
        {
            dtoRole = input;
        }
    }
}
