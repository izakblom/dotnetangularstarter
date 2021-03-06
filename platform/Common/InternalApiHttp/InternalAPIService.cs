using Common.DataObjects.Authentication;
using Common.Exceptions;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Common.InternalApiHttp
{
    public class InternalAPIService : IInternalAPIService
    {
        private IConfiguration _configuration;

        public InternalAPIService(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async Task<DTOToken> GetAPIAccessToken()
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

        public async Task<T> InternalAPIPostAsync<T, R>(string configApiEndpointKey, R requestBody, string queryString = "")
        {
            try
            {
                //Auth system user
                var token = await GetAPIAccessToken();

                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token.token}");
                    client.Timeout = new TimeSpan(0, 0, 60); //60 second timeout.


                    var task_pay_resp = await client.PostAsJsonAsync(_configuration[$"{Environment.GetEnvironmentVariable("TENANT")}:{configApiEndpointKey}"] + queryString, requestBody);
                    T result;



                    if (task_pay_resp.IsSuccessStatusCode)
                    {
                        var task_pay_resp_str = await task_pay_resp.Content.ReadAsStringAsync();
                        result = JsonConvert.DeserializeObject<T>(task_pay_resp_str);
                    }
                    else
                    {
                        throw new GeneralDomainException("API failure");
                    }

                    return result;

                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<bool> InternalAPIPostAsync<R>(string configApiEndpointKey, R requestBody)
        {
            try
            {
                //Auth system user
                var token = await GetAPIAccessToken();

                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token.token}");
                    client.Timeout = new TimeSpan(0, 0, 60); //60 second timeout.


                    var task_pay_resp = await client.PostAsJsonAsync(_configuration[$"{Environment.GetEnvironmentVariable("TENANT")}:{configApiEndpointKey}"], requestBody);


                    bool result = false;
                    if (task_pay_resp.IsSuccessStatusCode)
                    {
                        result = true;
                    }
                    else
                    {
                        throw new GeneralDomainException("API failure");
                    }
                    return result;

                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<T> InternalAPIGetAsync<T>(string configApiEndpointKey, string queryString = "")
        {
            try
            {
                //Auth system user
                var token = await GetAPIAccessToken();

                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token.token}");
                    client.Timeout = new TimeSpan(0, 0, 60); //60 second timeout.


                    var task_pay_resp = await client.GetAsync(_configuration[$"{Environment.GetEnvironmentVariable("TENANT")}:{configApiEndpointKey}"] + queryString);
                    T result;

                    if (task_pay_resp.IsSuccessStatusCode)
                    {
                        var task_pay_resp_str = await task_pay_resp.Content.ReadAsStringAsync();
                        result = JsonConvert.DeserializeObject<T>(task_pay_resp_str);
                    }
                    else
                    {
                        throw new GeneralDomainException("API failure");
                    }

                    return result;

                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<T> InternalAPIPostAsync<T, R>(string configApiEndpointKey, string routeParams, R requestBody, string queryString = "")
        {
            try
            {
                //Auth system user
                var token = await GetAPIAccessToken();

                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token.token}");
                    client.Timeout = new TimeSpan(0, 0, 60); //60 second timeout.

                    var task_pay_resp = await client.PostAsJsonAsync(_configuration[$"{Environment.GetEnvironmentVariable("TENANT")}:{configApiEndpointKey}"] + routeParams + queryString, requestBody);
                    T result;



                    if (task_pay_resp.IsSuccessStatusCode)
                    {
                        var task_pay_resp_str = await task_pay_resp.Content.ReadAsStringAsync();
                        result = JsonConvert.DeserializeObject<T>(task_pay_resp_str);
                    }
                    else
                    {
                        throw new GeneralDomainException("API failure");
                    }

                    return result;

                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
