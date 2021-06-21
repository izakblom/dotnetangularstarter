using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using dotnetstarter.organisations.domain.AggregatesModel.UserAggregate;
using dotnetstarter.organisations.common.DataObjects;
using Common.Exceptions;
using dotnetstarter.organisations.tickers;
using Microsoft.AspNetCore.Http;

namespace dotnetstarter.organisations.api.Application.Commands.Tickers
{
    public class BuildTickersCommandHandler : IRequestHandler<BuildTickersCommand, Dictionary<TickerCategory, List<Ticker>>>
    {
        private readonly IUserRepository _userRepository;
        private User user;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public BuildTickersCommandHandler(IUserRepository userRepository, IHttpContextAccessor httpContextAccessor)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        public async Task<Dictionary<TickerCategory, List<Ticker>>> Handle(BuildTickersCommand request, CancellationToken cancellationToken)
        {
            int coreUserIdRef = (int)_httpContextAccessor.HttpContext.Items["Req-User-Id"];
            user = await _userRepository.FindByCoreUserIdRef(coreUserIdRef);
            if (user == null)
                throw new UserNotFoundException();

            var res = new Dictionary<TickerCategory, List<Ticker>>();

            var userPermIds = user.UserPermissions.Select(up => up.Permission.Id).ToList();

            TickerCategory.List().ToList().ForEach(c =>
            {
                var tickers = dotnetstarter.organisations.tickers.Tickers.All.Where(t =>
                                  t.Category == c &&
                                  t.Permissions.All(p => userPermIds.Contains(p.Id))
                                ).ToList(); //make sure the user has all the required permissions.

                res.Add(c, tickers);
            });

            return res;
        }

    }
}
