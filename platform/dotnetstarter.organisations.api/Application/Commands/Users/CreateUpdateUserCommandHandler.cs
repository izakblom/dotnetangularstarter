using Common.Exceptions;
using dotnetstarter.organisations.domain.AggregatesModel.UserAggregate;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace dotnetstarter.organisations.api.Application.Commands.Users
{
    public class CreateUpdateUserCommandHandler : IRequestHandler<CreateUpdateUserCommand, bool>
    {
        private readonly IUserRepository _userRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMediator _mediator;

        public CreateUpdateUserCommandHandler(IUserRepository userRepository, IHttpContextAccessor httpContextAccessor, IMediator mediator)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Handle(CreateUpdateUserCommand request, CancellationToken cancellationToken)
        {
            try
            {

                var user = await _userRepository.FindByCoreUserIdRef(request.dtoInput.CoreUserIdRef);
                bool saved = false;
                if (user != null)
                {
                    //user.AddEmail(request.dtoInput.Email);
                    user.AddNames(request.dtoInput.FirstName, request.dtoInput.LastName);
                    user.AddIDNumber(request.dtoInput.IDNumber);
                    user.AddMobileNumber(request.dtoInput.MobileNumber);
                    _userRepository.Update(user);
                    saved = await _userRepository.UnitOfWork.SaveEntitiesAsync();
                }
                else
                {


                    user = new User(request.dtoInput.FirstName, request.dtoInput.LastName, request.dtoInput.IDNumber, request.dtoInput.MobileNumber, request.dtoInput.Email,
                         request.dtoInput.CoreUserIdRef);
                    _userRepository.Add(user);
                    saved = await _userRepository.UnitOfWork.SaveEntitiesAsync();


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
