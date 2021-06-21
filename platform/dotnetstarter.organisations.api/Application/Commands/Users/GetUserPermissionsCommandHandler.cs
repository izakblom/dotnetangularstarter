using Common.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using dotnetstarter.organisations.common.DataObjects;
using dotnetstarter.organisations.domain.AggregatesModel.UserAggregate;

namespace dotnetstarter.organisations.api.Application.Commands.Users
{
    public class GetUserPermissionsCommandHandler : IRequestHandler<GetUserPermissionsCommand, List<DTOPermission>>
    {
        private readonly IUserRepository userRepository;
        private readonly IHttpContextAccessor httpContextAccessor;

        public GetUserPermissionsCommandHandler(IUserRepository userRepository, IHttpContextAccessor httpContextAccessor)
        {
            this.userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            this.httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        public async Task<List<DTOPermission>> Handle(GetUserPermissionsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                int coreUserIdRef = (int)httpContextAccessor.HttpContext.Items["Req-User-Id"];
                var user = await userRepository.FindByCoreUserIdRef(coreUserIdRef);
                if (user == null)
                    throw new UserNotFoundException();
                var result = new List<DTOPermission>();
                foreach (var userPerm in user.UserPermissions)
                {
                    result.Add(new DTOPermission { Id = userPerm.Permission.Id, Name = userPerm.Permission.Name });
                }
                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
