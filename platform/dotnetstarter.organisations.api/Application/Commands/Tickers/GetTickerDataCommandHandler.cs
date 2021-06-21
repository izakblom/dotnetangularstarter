using Common.DataObjects.Ticker;
using Common.Exceptions;
using dotnetstarter.organisations.common.DataObjects.Tickers;
using dotnetstarter.organisations.domain.AggregatesModel.UserAggregate;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static dotnetstarter.organisations.tickers.Tickers;

namespace dotnetstarter.organisations.api.Application.Commands.Tickers
{
    public class GetTickerDataCommandHandler : IRequestHandler<GetTickerDataCommand, ITickerResult>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMediator _mediator;
        private User user;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetTickerDataCommandHandler(IUserRepository userRepository, IHttpContextAccessor httpContextAccessor, IMediator mediator)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(IMediator));
        }

        public async Task<ITickerResult> Handle(GetTickerDataCommand request, CancellationToken cancellationToken)
        {
            try
            {
                int coreUserIdRef = (int)_httpContextAccessor.HttpContext.Items["Req-User-Id"];
                user = await _userRepository.FindByCoreUserIdRef(coreUserIdRef);
                if (user == null)
                    throw new UserNotFoundException();

                var userPermIds = user.UserPermissions.Select(up => up.Permission.Id).ToList();

                //find selected ticker and determine if user has access to the ticker via assigned permissions.
                var selectedTicker = All.FirstOrDefault(t => t.Id == request.tickerFilter.templateId);

                if (!selectedTicker.Permissions.Select(p => p.Id).All(p => userPermIds.Contains(p)))
                    throw new Exception("Not authorized to access this ticker!");

                var result = 0;

                

                return new DTOTickerCountResult() { Count = result };
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}