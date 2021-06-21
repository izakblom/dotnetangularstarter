using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using dotnetstarter.organisations.domain.AggregatesModel.RoleAggregate;

namespace dotnetstarter.organisations.api.Application.Commands.Auth
{
    public class GetAllFeatureGuardsCommandHandler : IRequestHandler<GetAllFeatureGuardsCommand, List<FeatureGuard>>
    {
        public async Task<List<FeatureGuard>> Handle(GetAllFeatureGuardsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                return FeaturePermissions.ALL;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
