using Common.DataObjects.Authentication;
using System.Threading.Tasks;

namespace Common.InternalApiHttp
{
    public interface IInternalAPIService
    {
        Task<T> InternalAPIPostAsync<T, R>(string configApiEndpointKey, R requestBody, string queryString = "");
        Task<T> InternalAPIPostAsync<T, R>(string configApiEndpointKey, string routeParams, R requestBody, string queryString = "");
        Task<bool> InternalAPIPostAsync<R>(string configApiEndpointKey, R requestBody);
        Task<T> InternalAPIGetAsync<T>(string configApiEndpointKey, string queryString = "");
        Task<DTOToken> GetAPIAccessToken();
    }
}
