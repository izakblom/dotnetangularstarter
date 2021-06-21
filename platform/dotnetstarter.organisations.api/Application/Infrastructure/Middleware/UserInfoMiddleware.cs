using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotnetstarter.organisations.domain.AggregatesModel.UserAggregate;

namespace dotnetstarter.organisations.api.Application.Middleware
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
                //Retrieve the user id from request headers
                string userId = httpContextAccessor.HttpContext.Request.Headers["X-Initiating-User-Id"];
                if (!string.IsNullOrEmpty(userId))
                {
                    int uid;
                    if (int.TryParse(userId, out uid))
                    {
                        httpContextAccessor.HttpContext.Items["Req-User-Id"] = uid;
                        var user = await userRepository.FindByCoreUserIdRef(uid);
                        httpContextAccessor.HttpContext.Items["User"] = user;
                    }
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
