using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Common.Utilities.FirebaseREST
{
    class FirebaseRestService : IFirebaseRestService
    {
        private string webAPIKey;
        private string firebaseRESTAPI;
        private readonly JsonSerializerSettings jsonSettings = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            MissingMemberHandling = MissingMemberHandling.Ignore
        };

        public FirebaseRestService(IConfiguration configuration)
        {
            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));
            webAPIKey = configuration["Firebase:WebApiKey"];
            if (String.IsNullOrEmpty(webAPIKey))
                throw new Exception("Invalid firebase key");

            firebaseRESTAPI = $"https://www.googleapis.com/identitytoolkit/v3/relyingparty/setAccountInfo?key={webAPIKey}";
        }

        //public bool DisableFirebaseUser(string UID)
        //{

        //}

        public async Task<TResponse> Post<TResponse>(string endpoint, object request)
        {
            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    var content = JsonConvert.SerializeObject(request, jsonSettings);
                    var payload = new StringContent(content, Encoding.UTF8, "application/json");
                    var response = await httpClient.PostAsync(endpoint, payload);
                    var responseJson = await response.Content.ReadAsStringAsync();
                    response.EnsureSuccessStatusCode();
                    return JsonConvert.DeserializeObject<TResponse>(responseJson);
                }

            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}
