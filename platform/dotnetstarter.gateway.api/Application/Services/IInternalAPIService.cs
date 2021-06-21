using System.Threading.Tasks;

namespace dotnetstarter.gateway.api.Application.Services
{
    public interface IInternalAPIService
    {
        Task<T> InternalAPIPostAsync<T, R>(string configApiEndpointKey, R requestBody, string queryString = "");
        Task<T> InternalAPIPostAsync<T, R>(string configApiEndpointKey, string routeParams, R requestBody, string queryString = "");
        Task<bool> InternalAPIPostAsync<R>(string configApiEndpointKey, R requestBody, string queryString = "");
        Task<T> InternalAPIGetAsync<T>(string configApiEndpointKey, string queryString = "");
    }
}
