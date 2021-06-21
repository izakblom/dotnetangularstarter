using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using dotnetstarter.organisations.common.DataObjects;
using dotnetstarter.organisations.domain.AggregatesModel.UserAggregate;

namespace dotnetstarter.organisations.api.Application.Commands.Admin.Users
{
    public class GetUserWithPermissionsCommandHandler : IRequestHandler<GetUserWithPermissionsCommand, DTOUser>
    {
        private IUserRepository _userRepository;

        public GetUserWithPermissionsCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        public async Task<DTOUser> Handle(GetUserWithPermissionsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _userRepository.FindByIdAsync(request.userId);
                var result = new DTOUser
                {
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Id = user.Id.ToString(),
                    Mobile = user.MobileNumber,
                    IsActive = user.IsActive
                };
                result.Permissions = new List<DTOPermission>();
                foreach (var permission in user.UserPermissions)
                    result.Permissions.Add(new DTOPermission { Id = permission.Permission.Id, Name = permission.Permission.Name });
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
