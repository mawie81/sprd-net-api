using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using sprd_net_api.exception;

namespace sprd_net_api.client
{
    public class ApiClient
    {
        private CookieContainer _cookieContainer;
        private HttpClientHandler _handler;
        private bool _isInitialized;
        private static ApiClient _instance = new ApiClient();

        private ApiClient()
        {
            
        }

        public static ApiClient Instance { get { return _instance;  } }

        private HttpClientHandler Handler
        {
            get
            {
                return _handler;
            }
        }

        private HttpClient GetClient()
        {
            // the second param prevents the Handler from being disposed with the client
            var client = new HttpClient(Handler, false);
            
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return client;
        }

        private Uri CreateUri(string path)
        {
            var builder = new UriBuilder(path);
            builder.Query += "mediaType=json&locale=de_DE";
            return builder.Uri;
        }

        public bool Init(HttpClientHandler handler)
        {
            _handler = handler;
            _handler.CookieContainer = new CookieContainer();
            _handler.UseCookies = true;
            
            _isInitialized = true;
            return _isInitialized;
        }

        public async Task<T> Get<T>(string path) where T : class
        {
            using (var client = GetClient())
            {
                var content = await client.GetStringAsync(path);
                return JsonConvert.DeserializeObject<T>(content);
            }
        }

        public async Task Post<TRequest>(string path, TRequest data)  where TRequest : class
        {
            using (var client = GetClient())
            {
                var serializedData = JsonConvert.SerializeObject(data);
                var result = await client.PostAsync(CreateUri(path), new StringContent(serializedData));
                if (result.StatusCode != HttpStatusCode.Created)
                {
                    var resultContent = await result.Content.ReadAsStringAsync();
                    throw new ApiException("Could not post data.", resultContent);
                }
            }
        }
    }
}
