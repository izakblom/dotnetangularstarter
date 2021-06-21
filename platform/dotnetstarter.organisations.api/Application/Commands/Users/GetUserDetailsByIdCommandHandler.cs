using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using dotnetstarter.organisations.domain.AggregatesModel.UserAggregate;

namespace dotnetstarter.organisations.api.Application.Commands.Users
{
    public class GetUserDetailsByIdCommandHandler : IRequestHandler<GetUserDetailsByIdCommand, object>
    {
        private readonly IUserRepository _userRepository;

        public GetUserDetailsByIdCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(IUserRepository));
        }

        public async Task<object> Handle(GetUserDetailsByIdCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _userRepository.GetUserByIdForCommissionApi(request._userId);
                return user;
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}