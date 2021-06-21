using Common.DataObjects.Authentication;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotnetstarter.gateway.api.DataObjects;

namespace dotnetstarter.gateway.api.Application.Commands.Core
{
    public class GetAPIAccessTokenCommand : IRequest<DTOToken>
    {
    }
}
