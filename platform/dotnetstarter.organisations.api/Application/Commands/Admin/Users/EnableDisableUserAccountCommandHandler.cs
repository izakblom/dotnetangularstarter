using Common.Exceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using dotnetstarter.organisations.domain.AggregatesModel.UserAggregate;

namespace dotnetstarter.organisations.api.Application.Commands.Admin.Users
{
    public class EnableDisableUserAccountCommandHandler : IRequestHandler<EnableDisableUserAccountCommand, int>
    {
        IUserRepository _userRepository;

        public EnableDisableUserAccountCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        public async Task<int> Handle(EnableDisableUserAccountCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Find the user
                var user = await _userRepository.FindByIdAsync(request.inputDTO.UserId);
                if (user == null)
                    throw new UserNotFoundException();

                if (user.IsActive)
                    user.DisableUser();
                else
                    user.EnableUser();

                _userRepository.Update(user);
                bool updated = await _userRepository.UnitOfWork.SaveEntitiesAsync();

                return user.CoreUserIdRef;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
