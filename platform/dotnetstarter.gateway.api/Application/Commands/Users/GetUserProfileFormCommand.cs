using Common.Utilities.CustomAttributes;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotnetstarter.gateway.api.Application.Commands.Users
{
    public class GetUserProfileFormCommand : IRequest<DTODynamicFormsStructure>
    {

        public readonly string ClaimsUserEmail;
        public readonly string ClaimsUserJWTId;

        public GetUserProfileFormCommand(string jwtId, string email)
        {
            ClaimsUserJWTId = jwtId;
            ClaimsUserEmail = email;
        }
    }
}
