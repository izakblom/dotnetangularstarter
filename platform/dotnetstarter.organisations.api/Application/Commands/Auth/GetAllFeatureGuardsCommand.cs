using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotnetstarter.organisations.domain.AggregatesModel.RoleAggregate;

namespace dotnetstarter.organisations.api.Application.Commands.Auth
{
    public class GetAllFeatureGuardsCommand : IRequest<List<FeatureGuard>>
    {
    }
}
