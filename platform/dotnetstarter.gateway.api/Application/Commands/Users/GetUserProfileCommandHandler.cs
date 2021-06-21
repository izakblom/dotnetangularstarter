using Common.Logging;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using dotnetstarter.gateway.domain.AggregatesModel.UserAggregate;
using dotnetstarter.gateway.api.DataObjects;
using Common.Exceptions;
using dotnetstarter.organisations.common.DataObjects;
using DTOUserProfile = dotnetstarter.gateway.api.DataObjects.DTOUserProfile;
using Microsoft.Extensions.Configuration;
using dotnetstarter.gateway.api.Application.Services;

namespace dotnetstarter.gateway.api.Application.Commands.Users
{
    public class GetUserProfileCommandHandler : IRequestHandler<GetUserProfileCommand, DTOUserProfile>
    {
        private readonly ICustomLogger _customLogger;
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;
        private readonly IInternalAPIService _internalApiService;

        public GetUserProfileCommandHandler(ICustomLogger customLogger, IUserRepository userRepository, IConfiguration configuration, IInternalAPIService internalAPIService)
        {
            _customLogger = customLogger ?? throw new ArgumentNullException(nameof(customLogger));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _internalApiService = internalAPIService ?? throw new ArgumentNullException(nameof(internalAPIService));
        }

        public async Task<DTOUserProfile> Handle(GetUserProfileCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _userRepository.FindByProviderUID(request.claimsUserJWTId);
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
                    JWTId = request.claimsUserJWTId,
                    Complete = user.ProfileComplete(),
                    Id = user.Id.ToString()
                };

                //Query organisations api for user permissions
                List<DTOPermission> permissions = await _internalApiService.InternalAPIGetAsync<List<DTOPermission>>("OrganisationsAPI:GetUserPermissions");
                profile.Permissions = permissions;

                return profile;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
