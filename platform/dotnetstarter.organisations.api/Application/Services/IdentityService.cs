using System;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace dotnetstarter.organisations.api.Application.Infrastructure.services
{
    public class IdentityService : IIdentityService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public IdentityService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetUserPublicKey()
        {

            try
            {
                var context = _httpContextAccessor.HttpContext;

                var hasClaim = context.User.HasClaim(c => c.Type == "PublicKey");

                if (!hasClaim)
                    throw new UnauthorizedAccessException($"Could not identify user: missing public key claim!");

                return context.User.Claims.FirstOrDefault(c => c.Type == "PublicKey").Value;

            }
            catch (Exception ex)
            {
                throw new UnauthorizedAccessException(ex.Message);
            }



        }

        public string GetUserName()
        {

            try
            {
                var context = _httpContextAccessor.HttpContext;

                var hasClaim = context.User.HasClaim(c => c.Type == "Username");

                if (!hasClaim)
                    throw new UnauthorizedAccessException($"Could not identify user: missing username claim!");

                return context.User.Claims.FirstOrDefault(c => c.Type == "Username").Value;

            }
            catch (Exception ex)
            {
                throw new UnauthorizedAccessException(ex.Message);
            }



        }
    }
}
