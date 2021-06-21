using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotnetstarter.organisations.common.DataObjects.Admin;

namespace dotnetstarter.organisations.api.Application.Commands.Admin.Users
{
    public class EnableDisableUserAccountCommand : IRequest<int>
    {
        public DTOEnableDisableUserAccount inputDTO { get; set; }

        public EnableDisableUserAccountCommand(DTOEnableDisableUserAccount inputDTO)
        {
            this.inputDTO = inputDTO;
        }
    }
}
