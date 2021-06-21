using dotnetstarter.authentication.domain.AggregatesModel.IdentityAggregate;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace dotnetstarter.authentication.api.Application.Commands
{
    public class CreateUserCommandHandler
    : IRequestHandler<CreateUserCommand, bool>
    {
        private readonly IIdentityRepository _identityRepository;
        private readonly IMediator _mediator;

        // Using DI to inject infrastructure persistence Repositories
        public CreateUserCommandHandler(IMediator mediator,
                                         IIdentityRepository identityRepository)
        {

            _identityRepository = identityRepository ??
                              throw new ArgumentNullException(nameof(identityRepository));

            _mediator = mediator ??
                                     throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var user = new Identity(request.Username);
            _identityRepository.Add(user);

            var saved = await _identityRepository.UnitOfWork.SaveEntitiesAsync();

            if (!saved)
                throw new Exception($"Could not create a new authentication api user {request.Username}.");

            return true;
        }
    }
}
