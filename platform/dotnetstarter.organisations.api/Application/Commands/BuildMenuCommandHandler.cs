using Common.Exceptions;
using dotnetstarter.organisations.common.DataObjects;
using dotnetstarter.organisations.domain.AggregatesModel.RoleAggregate;
using dotnetstarter.organisations.domain.AggregatesModel.UserAggregate;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace dotnetstarter.organisations.api.Application.Commands
{
    public class BuildMenuCommandHandler : IRequestHandler<BuildMenuCommand, DTOMenuItem[]>
    {
        private readonly IUserRepository _userRepository;
        private User user;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public BuildMenuCommandHandler(IUserRepository userRepository, IHttpContextAccessor httpContextAccessor)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        public async Task<DTOMenuItem[]> Handle(BuildMenuCommand request, CancellationToken cancellationToken)
        {
            int coreUserIdRef = (int)_httpContextAccessor.HttpContext.Items["Req-User-Id"];
            user = await _userRepository.FindByCoreUserIdRef(coreUserIdRef);
            if (user == null)
                throw new UserNotFoundException();

            List<DTOMenuItem> result = new List<DTOMenuItem>();

            // Only return menu items if the user has not been disabled
            if (user.IsActive)
            {


                //Profile
                result.Add(buildProfileMenuItem());



                //Administration
                if (user.HasPermission(Permission.ACCESS_ADMINISTRATION_BACKOFFICE))
                    result.Add(buildAdminMenuItem());

            }

            return result.ToArray();
        }

        private DTOMenuItem buildHomeMenuItem()
        {
            var home = new DTOMenuItem("Home", "", "home");
            //var homeChild1 = new DTOMenuItem("HomeChild1", "");
            //homeChild1.children.Add(new DTOMenuItem("HomeChild11", ""));
            //homeChild1.children.Add(new DTOMenuItem("HomeChild12", ""));
            //home.children.Add(homeChild1);
            //home.children.Add(new DTOMenuItem("HomeChild2", ""));
            //home.children.Add(new DTOMenuItem("HomeChild3", ""));
            return home;
        }




        private DTOMenuItem buildProfileMenuItem()
        {
            var profile = new DTOMenuItem("Profile", "/profile", "user");
            return profile;
        }



        private DTOMenuItem buildAdminMenuItem()
        {
            var administration = new DTOMenuItem("Administration", "/admin", "shield-check");

            administration.children.Add(new DTOMenuItem("Dashboard", "/admin/dashboard", "pie-chart"));
            if (user.HasPermission(Permission.VIEW_USERS))
                administration.children.Add(new DTOMenuItem("Users", "/admin/users", "user"));
            if (user.HasPermission(Permission.MANAGE_ROLES))
                administration.children.Add(new DTOMenuItem("Roles", "/admin/roles", "lock"));

            return administration;
        }
    }
}
