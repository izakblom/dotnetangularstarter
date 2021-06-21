using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using dotnetstarter.organisations.domain.AggregatesModel.RoleAggregate;

namespace dotnetstarter.organisations.api.Application.Commands.Admin.Roles
{
    public class CreateEditRoleCommandHandler : IRequestHandler<CreateEditRoleCommand, bool>
    {
        private readonly IRoleRepository roleRepository;

        public CreateEditRoleCommandHandler(IRoleRepository roleRepository)
        {
            this.roleRepository = roleRepository ?? throw new ArgumentNullException(nameof(roleRepository));
        }

        public async Task<bool> Handle(CreateEditRoleCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (string.IsNullOrEmpty(request.dtoRole.Name) || string.IsNullOrEmpty(request.dtoRole.Description) || request.dtoRole.Permissions.Count == 0)
                    throw new Exception("Invalid role create-update command");
                bool saved = false;
                if (request.dtoRole.Id == -1) //Create role
                {
                    var role = new Role(request.dtoRole.Name, request.dtoRole.Description);
                    foreach (var permission in request.dtoRole.Permissions)
                    {
                        var addPerm = Permission.From(permission.Id);
                        role.AddPermission(addPerm);
                    }
                    roleRepository.Add(role);

                }
                else //edit role
                {
                    var role = await roleRepository.FindByIdAsync(request.dtoRole.Id);
                    if (role == null)
                        throw new Exception("Role not found");
                    role.SetName(request.dtoRole.Name);
                    role.SetDescription(request.dtoRole.Description);
                    //remove the permissions not included
                    var rolePermsBeforeMod = role.RolePermissions.ToList();
                    foreach (var rolePerm in rolePermsBeforeMod)
                    {
                        if (request.dtoRole.Permissions.Find(dtoPerm => dtoPerm.Id == rolePerm.Permission.Id) == null)

                            role.RemovePermission(rolePerm.Permission);
                    }
                    //add all the permissions from the dto, AddPermission checks for duplicates
                    foreach (var dtoPerm in request.dtoRole.Permissions)
                    {
                        var addPerm = Permission.From(dtoPerm.Id);
                        role.AddPermission(addPerm);
                    }
                    roleRepository.Update(role);
                }
                saved = await roleRepository.UnitOfWork.SaveEntitiesAsync();
                return saved;
            }
            catch (Exception)
            {

                throw;
            }

        }
    }
}
