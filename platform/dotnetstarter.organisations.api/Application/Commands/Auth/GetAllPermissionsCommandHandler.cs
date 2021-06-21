using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using dotnetstarter.organisations.common.DataObjects;
using dotnetstarter.organisations.domain.AggregatesModel.RoleAggregate;

namespace dotnetstarter.organisations.api.Application.Commands.Auth
{
    public class GetAllPermissionsCommandHandler : IRequestHandler<GetAllPermissionsCommand, List<DTOPermission>>
    {
        public async Task<List<DTOPermission>> Handle(GetAllPermissionsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var perms = Permission.List();
                var result = new List<DTOPermission>();
                foreach (var perm in perms)
                {
                    result.Add(new DTOPermission
                    {
                        Id = perm.Id,
                        Name = perm.Name
                    });
                }
                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
