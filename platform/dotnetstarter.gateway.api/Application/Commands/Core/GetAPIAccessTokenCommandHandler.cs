using Common.DataObjects.Authentication;
using MediatR;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace dotnetstarter.gateway.api.Application.Commands.Core
{
    public class GetAPIAccessTokenCommandHandler : IRequestHandler<GetAPIAccessTokenCommand, DTOToken>
    {
        private IConfiguration _configuration;

        public GetAPIAccessTokenCommandHandler(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async Task<DTOToken> Handle(GetAPIAccessTokenCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var token = new DTOToken();

                //authenticate backend request.
                using (HttpClient client = new HttpClient())
                {
                    var tokenReq = new DTOTokenRequest();
                    tokenReq.Username = _configuration[$"{Environment.GetEnvironmentVariable("TENANT")}:Identity:Username"];
                    tokenReq.Password = _configuration[$"{Environment.GetEnvironmentVariable("TENANT")}:Identity:Password"];

                    //get the auth token.
                    var resp = await client.PostAsJsonAsync<DTOTokenRequest>($"{_configuration[$"{Environment.GetEnvironmentVariable("TENANT")}:Identity:RequestTokenAddress"]}", tokenReq);
                    var respstr = await resp.Content.ReadAsStringAsync();

                    //got the authentication token.
                    token = JsonConvert.DeserializeObject<DTOToken>(respstr);

                    return token;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
