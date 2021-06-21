using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotnetstarter.organisations.common.DataObjects;

namespace dotnetstarter.organisations.api.Application.Commands.Users
{
    public class CreateUpdateUserCommand : IRequest<bool>
    {
        public DTOUserProfile dtoInput { get; set; }

        public CreateUpdateUserCommand(DTOUserProfile input)
        {
            dtoInput = input;
        }
    }
}
