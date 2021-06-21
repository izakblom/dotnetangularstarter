using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotnetstarter.gateway.api.DataObjects.Admin;

namespace dotnetstarter.gateway.api.Application.Commands.Admin
{
    public class EnableDisableUserAccountCommand : IRequest<bool>
    {
        public DTOEnableDisableUserAccount inputDTO { get; set; }

        public EnableDisableUserAccountCommand(DTOEnableDisableUserAccount inputDTO)
        {
            this.inputDTO = inputDTO;
        }
    }
}
