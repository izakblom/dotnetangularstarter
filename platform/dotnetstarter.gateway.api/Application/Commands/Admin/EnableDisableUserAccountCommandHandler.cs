using Common.Exceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using dotnetstarter.gateway.api.Application.Services;
using dotnetstarter.gateway.api.DataObjects;
using dotnetstarter.gateway.domain.AggregatesModel.UserAggregate;

namespace dotnetstarter.gateway.api.Application.Commands.Admin
{
    public class EnableDisableUserAccountCommandHandler : IRequestHandler<EnableDisableUserAccountCommand, bool>
    {
        private readonly IUserRepository _userRepository;
        private readonly IInternalAPIService _internalAPIService;

        public EnableDisableUserAccountCommandHandler(IUserRepository userRepository, IInternalAPIService internalAPIService)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _internalAPIService = internalAPIService ?? throw new ArgumentNullException(nameof(internalAPIService));
        }

        public async Task<bool> Handle(EnableDisableUserAccountCommand request, CancellationToken cancellationToken)
        {
            try
            {
                //relay to organisation.api
                dotnetstarter.organisations.common.DataObjects.Admin.DTOEnableDisableUserAccount dto
                    = new dotnetstarter.organisations.common.DataObjects.Admin.DTOEnableDisableUserAccount
                    {
                        UserId = int.Parse(request.inputDTO.UserId)
                    };
                int coreUserIdRef = await _internalAPIService.InternalAPIPostAsync<int, dotnetstarter.organisations.common.DataObjects.Admin.DTOEnableDisableUserAccount>(
                    "OrganisationsAPI:EnableDisableUserAccount", dto);

                // Find the user
                var user = await _userRepository.FindByIdAsync(coreUserIdRef);
                if (user == null)
                    throw new UserNotFoundException();

                if (user.IsActive)
                    user.DisableUser();
                else
                    user.EnableUser();

                _userRepository.Update(user);
                bool updated = await _userRepository.UnitOfWork.SaveEntitiesAsync();


                return updated;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
