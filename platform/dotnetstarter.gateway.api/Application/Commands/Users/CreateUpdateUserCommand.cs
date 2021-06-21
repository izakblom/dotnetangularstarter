using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotnetstarter.gateway.api.DataObjects;

namespace dotnetstarter.gateway.api.Application.Commands.Users
{
    public class CreateUpdateUserCommand : IRequest<bool>
    {
        public DTOUserProfile dtoInput { get; set; }
        public string CurrentUserJwtId { get; set; }
        public string CurrentUserEmail { get; set; }

        public CreateUpdateUserCommand(DTOUserProfile input, string jwtId, string email)
        {
            dtoInput = input;
            CurrentUserJwtId = jwtId;
            CurrentUserEmail = email;
        }
    }
}
