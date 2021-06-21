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
    public class AssignRevokePermissionCommandHandler : IRequestHandler<AssignRevokePermissionCommand, List<DTOPermission>>
    {
        IUserRepository _userRepository;

        public AssignRevokePermissionCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        public async Task<List<DTOPermission>> Handle(AssignRevokePermissionCommand request, CancellationToken cancellationToken)
        {
            try
            {
                //find the user
                var user = await _userRepository.FindByIdAsync(request.inputDTO.userId);
                if (user == null)
                    throw new UserNotFoundException("User does not exist");
                //find the permission
                var permission = Permission.List().First(perm => { return perm.Id == request.inputDTO.permissionId; });
                if (permission == null)
                    throw new GeneralDomainException("Permission not found. Cannot assign or revoke");

                user.AssignRevokePermission(permission);
                //Update context
                _userRepository.Update(user);
                bool saved = await _userRepository.UnitOfWork.SaveEntitiesAsync();
                if (!saved)
                    throw new GeneralDomainException("Permission update failure");
                user = await _userRepository.FindByIdAsync(user.Id);
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
