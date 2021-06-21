using Common.Logging;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using dotnetstarter.organisations.domain.AggregatesModel.UserAggregate;
using dotnetstarter.organisations.common.DataObjects;
using Common.Exceptions;
using Microsoft.AspNetCore.Http;

namespace dotnetstarter.organisations.api.Application.Commands.Users
{
    public class GetUserProfileCommandHandler : IRequestHandler<GetUserProfileCommand, DTOUserProfile>
    {
        ICustomLogger _customLogger;
        IUserRepository _userRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetUserProfileCommandHandler(ICustomLogger customLogger, IUserRepository userRepository, IHttpContextAccessor httpContextAccessor)
        {
            _customLogger = customLogger ?? throw new ArgumentNullException(nameof(customLogger));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        public async Task<DTOUserProfile> Handle(GetUserProfileCommand request, CancellationToken cancellationToken)
        {
            try
            {
                int coreUserIdRef = (int)_httpContextAccessor.HttpContext.Items["Req-User-Id"];
                var user = await _userRepository.FindByCoreUserIdRef(coreUserIdRef);
                if (user == null) throw new UserNotFoundException();
                if (!user.IsActive)
                    throw new UserDisabledException("Profile has been disabled");

                DTOUserProfile profile = new DTOUserProfile
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    MobileNumber = user.MobileNumber,
                    IDNumber = user.IDNumber,
                    Email = user.Email,
                    Id = user.Id.ToString()
                };
                List<DTOPermission> permissions = new List<DTOPermission>();
                foreach (var userPermission in user.UserPermissions)
                    permissions.Add(new DTOPermission { Id = userPermission.Permission.Id, Name = userPermission.Permission.Name });
                profile.Permissions = permissions.ToArray();

                return profile;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
