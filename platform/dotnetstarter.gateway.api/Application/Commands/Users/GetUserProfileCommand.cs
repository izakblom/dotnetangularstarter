using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotnetstarter.gateway.api.DataObjects;

namespace dotnetstarter.gateway.api.Application.Commands.Users
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
