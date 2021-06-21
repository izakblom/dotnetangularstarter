using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotnetstarter.organisations.common.DataObjects;

namespace dotnetstarter.organisations.api.Application.Commands.Users
{
    public class GetUserProfileCommand : IRequest<DTOUserProfile>
    {
        public readonly string claimsUserJWTId;

        public GetUserProfileCommand(string jwtId)
        {
            claimsUserJWTId = jwtId;
        }
    }
}
