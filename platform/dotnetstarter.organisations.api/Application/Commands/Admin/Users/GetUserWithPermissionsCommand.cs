using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotnetstarter.organisations.common.DataObjects;

namespace dotnetstarter.organisations.api.Application.Commands.Admin.Users
{
    public class GetUserWithPermissionsCommand : IRequest<DTOUser>
    {
        public int userId { get; set; }

        public GetUserWithPermissionsCommand(int userId)
        {
            this.userId = userId;
        }
    }
}
