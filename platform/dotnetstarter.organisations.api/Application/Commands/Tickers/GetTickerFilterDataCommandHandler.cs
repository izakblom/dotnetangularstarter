using Common.Exceptions;
using Common.InternalApiHttp;
using dotnetstarter.organisations.common.DataObjects.Tickers;
using dotnetstarter.organisations.domain.AggregatesModel.UserAggregate;
using dotnetstarter.organisations.tickers;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace dotnetstarter.organisations.api.Application.Commands.Tickers
{
    public class GetTickerFilterDataCommandHandler : IRequestHandler<GetTickerFilterDataCommand, List<DTOOption>>
    {
        private readonly IUserRepository _userRepository;
        private User user;
        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly IInternalAPIService _internalApiService;

        public GetTickerFilterDataCommandHandler(
            IUserRepository userRepository,
            IHttpContextAccessor httpContextAccessor,
            IInternalAPIService internalApiService)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));

            _internalApiService = internalApiService ?? throw new ArgumentNullException(nameof(IInternalAPIService));
        }

        public async Task<List<DTOOption>> Handle(GetTickerFilterDataCommand request, CancellationToken cancellationToken)
        {
            int coreUserIdRef = (int)_httpContextAccessor.HttpContext.Items["Req-User-Id"];
            user = await _userRepository.FindByCoreUserIdRef(coreUserIdRef);
            if (user == null)
                throw new UserNotFoundException();

            var userPermIds = user.UserPermissions.Select(up => up.Permission.Id).ToList();

            //find selected ticker and determine if user has access to the ticker via assigned permissions.
            var selectedTicker = dotnetstarter.organisations.tickers.Tickers.All.FirstOrDefault(t => t.Id == request.ticker.Id);

            if (!selectedTicker.Permissions.Select(p => p.Id).All(p => userPermIds.Contains(p)))
                throw new Exception("Not authorized to access this ticker!");

            var res = new List<DTOOption>();

            if (request.filterType == TickerFilterType.DATERANGE)
            {
                var result = TickerDateRangeFilterType.List();
                res = result.Select(x => new DTOOption() { id = x.Id, value = x.Name }).ToList();
            }


            return res;
        }
    }
}