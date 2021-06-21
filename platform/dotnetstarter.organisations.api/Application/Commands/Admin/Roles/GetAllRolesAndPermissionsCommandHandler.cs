using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using dotnetstarter.organisations.api.Application.Queries;
using dotnetstarter.organisations.common.DataObjects;
using dotnetstarter.organisations.domain.AggregatesModel.RoleAggregate;
using dotnetstarter.organisations.domain.AggregatesModel.UserAggregate;

namespace dotnetstarter.organisations.api.Application.Commands.Admin.Roles
{
    public class GetAllRolesAndPermissionsCommandHandler : IRequestHandler<GetAllRolesAndPermissionsCommand, DTORolesAndPermissions>
    {
        private readonly IRoleRepository roleRepository;

        public GetAllRolesAndPermissionsCommandHandler(IRoleRepository roleRepository)
        {
            this.roleRepository = roleRepository ?? throw new ArgumentNullException(nameof(roleRepository));
        }

        public async Task<DTORolesAndPermissions> Handle(GetAllRolesAndPermissionsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var result = new DTORolesAndPermissions();
                var permissions = Permission.List();
                foreach (var permission in permissions)
                    result.permissions.Add(new DTOPermission { Id = permission.Id, Name = permission.Name });

                var roles = await roleRepository.FindAllAsync();
                foreach (var role in roles)
                {
                    var dto = new DTORole { Description = role.RoleDescription, Id = role.Id, Name = role.RoleName };
                    foreach (var rolePerm in role.RolePermissions)
                        dto.Permissions.Add(new DTOPermission { Id = rolePerm.Permission.Id, Name = rolePerm.Permission.Name });
                    result.roles.Add(dto);
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
