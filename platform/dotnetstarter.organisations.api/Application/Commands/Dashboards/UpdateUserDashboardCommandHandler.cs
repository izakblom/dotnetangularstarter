using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using dotnetstarter.organisations.domain.AggregatesModel.UserAggregate;
using dotnetstarter.organisations.common.DataObjects;
using Common.Exceptions;
using dotnetstarter.organisations.domain.AggregatesModel.DashboardAggregate;
using Microsoft.AspNetCore.Http;

namespace dotnetstarter.organisations.api.Application.Commands.Dashboards
{
    public class UpdateUserDashboardCommandHandler : IRequestHandler<UpdateUserDashboardCommand, bool>
    {
        private readonly IUserRepository _userRepository;
        private readonly IDashboardRepository _dashboardRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private User user;

        public UpdateUserDashboardCommandHandler(IUserRepository userRepository, IDashboardRepository dashboardRepository, IHttpContextAccessor httpContextAccessor)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _dashboardRepository = dashboardRepository ?? throw new ArgumentNullException(nameof(dashboardRepository));
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        public async Task<bool> Handle(UpdateUserDashboardCommand request, CancellationToken cancellationToken)
        {
            int coreUserIdRef = (int)_httpContextAccessor.HttpContext.Items["Req-User-Id"];
            user = await _userRepository.FindByCoreUserIdRef(coreUserIdRef);
            if (user == null)
                throw new UserNotFoundException();

            var dash = await _dashboardRepository.FindByUserIdAndRoute(user.Id, request.dtoInput.route);

            if (dash == null)
            {
                dash = new Dashboard(request.dtoInput.dashboard, request.dtoInput.route, user);
                _dashboardRepository.Add(dash);
            }
            else
            {
                dash.SetDashboard(request.dtoInput.dashboard);
                _dashboardRepository.Update(dash);
            }

            await _dashboardRepository.UnitOfWork.SaveEntitiesAsync();

            return true;

        }

    }
}
