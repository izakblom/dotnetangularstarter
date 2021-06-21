using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotnetstarter.gateway.domain.AggregatesModel.UserAggregate;

namespace dotnetstarter.gateway.api.Application.Middleware
{
    public class UserInfoMiddleware
    {
        private readonly RequestDelegate next;

        public UserInfoMiddleware(RequestDelegate next)
        {
            this.next = next;
        }



        public async Task Invoke(HttpContext context, IUserRepository userRepository, IHttpContextAccessor httpContextAccessor)
        {
            try
            {
                var userIdClaim = httpContextAccessor.HttpContext.User.FindFirst(c => c.Type == "user_id");
                if (userIdClaim != null)
                {
                    var user = await userRepository.FindByProviderUID(userIdClaim.Value);
                    httpContextAccessor.HttpContext.Items["User"] = user;
                }

                await next(context);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
