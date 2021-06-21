using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using dotnetstarter.organisations.common.DataObjects;
using dotnetstarter.organisations.domain.AggregatesModel.UserAggregate;

namespace dotnetstarter.organisations.api.Application.Commands.Users
{
    public class GetUserDetailsCommandHandler : IRequestHandler<GetUserDetailsCommand, DTOUser>
    {
        private readonly IUserRepository _userRepository;

        public GetUserDetailsCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        public async Task<DTOUser> Handle(GetUserDetailsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _userRepository.FindByIdAsync(request.UserId);
                if (user == null)
                    return null;
                return new DTOUser { Email = user.Email, FirstName = user.FirstName, LastName = user.LastName, Id = user.Id.ToString(), Mobile = user.MobileNumber };
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
