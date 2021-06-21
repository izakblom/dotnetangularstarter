using Common.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using dotnetstarter.gateway.api.Application.Services;
using dotnetstarter.gateway.domain.AggregatesModel.UserAggregate;
using dotnetstarter.organisations.common.DataObjects;

namespace dotnetstarter.gateway.api.Application.Commands.Users
{
    public class CreateUpdateUserCommandHandler : IRequestHandler<CreateUpdateUserCommand, bool>
    {
        private readonly IUserRepository _userRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMediator _mediator;
        private readonly IInternalAPIService _internalAPIService;

        public CreateUpdateUserCommandHandler(IUserRepository userRepository, IHttpContextAccessor httpContextAccessor,
            IMediator mediator, IInternalAPIService internalAPIService)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _internalAPIService = internalAPIService ?? throw new ArgumentNullException(nameof(internalAPIService));
        }

        public async Task<bool> Handle(CreateUpdateUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // adds a reference to the JWT userif one does not exist
                var user = await _userRepository.FindByProviderUID(request.CurrentUserJwtId);
                bool saved = false;
                bool newUser = false;
                DTOUserProfile beforeChange = null;
                if (user != null)
                {
                    beforeChange = new DTOUserProfile { Email = request.dtoInput.Email, IDNumber = request.dtoInput.IDNumber, FirstName = request.dtoInput.FirstName, LastName = request.dtoInput.LastName, MobileNumber = request.dtoInput.MobileNumber };
                    user.AddAlias(request.CurrentUserEmail, request.CurrentUserJwtId, request.dtoInput.Validated);
                    user.AddNames(request.dtoInput.FirstName, request.dtoInput.LastName);
                    user.AddIDNumber(request.dtoInput.IDNumber);
                    user.AddMobileNumber(request.dtoInput.MobileNumber);
                    _userRepository.Update(user);
                    saved = await _userRepository.UnitOfWork.SaveEntitiesAsync();
                }
                else
                {
                    newUser = true;

                    user = new User(request.dtoInput.FirstName, request.dtoInput.LastName, request.dtoInput.IDNumber, request.dtoInput.MobileNumber,
                        request.CurrentUserEmail, request.CurrentUserJwtId, request.dtoInput.Validated);
                    _userRepository.Add(user);
                    saved = await _userRepository.UnitOfWork.SaveEntitiesAsync();

                }
                if (saved)
                {
                    //relay to organisation.api
                    var dto = new DTOUserProfile
                    {
                        FirstName = request.dtoInput.FirstName,
                        LastName = request.dtoInput.LastName,
                        Email = request.CurrentUserEmail,
                        IDNumber = request.dtoInput.IDNumber,
                        MobileNumber = request.dtoInput.MobileNumber,
                        CoreUserIdRef = user.Id
                    };
                    bool apiResult = await _internalAPIService.InternalAPIPostAsync(
                        "OrganisationsAPI:CreateUpdateUser", dto);
                    if (!apiResult && newUser)
                    {
                        // Attempt to revert user creation in core
                        _userRepository.Delete(user);
                        await _userRepository.UnitOfWork.SaveEntitiesAsync();
                        throw new Exception("Organisations API failure");
                    }
                    else if (!apiResult && beforeChange != null)
                    {
                        // Attempt to revert user update in core
                        user.AddNames(beforeChange.FirstName, beforeChange.LastName);
                        user.AddIDNumber(beforeChange.IDNumber);
                        user.AddMobileNumber(beforeChange.MobileNumber);
                        _userRepository.Update(user);
                        saved = await _userRepository.UnitOfWork.SaveEntitiesAsync();
                    }
                }


                return saved;
            }
            catch (Exception ex)
            {
                throw new GeneralDomainException("Could not create user!");
            }
        }
    }
}
