using Common.Exceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using dotnetstarter.organisations.common.DataObjects;
using dotnetstarter.organisations.domain.AggregatesModel.RoleAggregate;
using dotnetstarter.organisations.domain.AggregatesModel.UserAggregate;

namespace dotnetstarter.organisations.api.Application.Commands.Admin.Users
{
    public class AssignRevokeRoleCommandHandler : IRequestHandler<AssignRevokeRoleCommand, List<DTOPermission>>
    {
        IUserRepository _userRepository;
        IRoleRepository _roleRepository;

        public AssignRevokeRoleCommandHandler(IUserRepository userRepository, IRoleRepository roleRepository)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _roleRepository = roleRepository ?? throw new ArgumentNullException(nameof(roleRepository));
        }

        public async Task<List<DTOPermission>> Handle(AssignRevokeRoleCommand request, CancellationToken cancellationToken)
        {
            try
            {
                //find the user
                var user = await _userRepository.FindByIdAsync(request.dtoInput.userId);
                if (user == null)
                    throw new UserNotFoundException("User does not exist");
                //find the role
                var role = await _roleRepository.FindByIdAsync(request.dtoInput.roleId);

                if (role == null)
                    throw new GeneralDomainException("Role not found. Cannot assign or revoke");

                //Determine if role is assigned by checking that all permissions in role are assigned
                bool allPermissions = true;

                foreach (var rolePerm in role.RolePermissions)
                {
                    if (!user.HasPermission(rolePerm.Permission))
                        allPermissions = false;
                }

                if (allPermissions) //Role is assigned, so revoke all permissions
                {
                    foreach (var rolePerm in role.RolePermissions)
                        user.RevokePermission(rolePerm.Permission);
                }
                else //Role is not assigned, assign all permissions
                {
                    foreach (var rolePerm in role.RolePermissions)
                        user.AssignPermission(rolePerm.Permission);
                }

                //Update context
                _userRepository.Update(user);
                bool saved = await _userRepository.UnitOfWork.SaveEntitiesAsync();
                if (!saved)
                    throw new GeneralDomainException("Role update failure");
                //Return the user permissions
                var result = new List<DTOPermission>();
                foreach (var userPermission in user.UserPermissions)
                    result.Add(new DTOPermission { Id = userPermission.Permission.Id, Name = userPermission.Permission.Name });
                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
