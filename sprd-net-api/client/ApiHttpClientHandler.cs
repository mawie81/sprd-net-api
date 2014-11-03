using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Utilities.Crypt;

namespace sprd_net_api.client
{
    public class ApiHttpClientHandler : HttpClientHandler
    {
        private readonly string _basePath;
        private readonly string _apiKey;
        private readonly string _apiSecret;

        public ApiHttpClientHandler(string basePath, string apiKey, string apiSecret)
        {
            _basePath = basePath;
            _apiKey = apiKey;
            _apiSecret = apiSecret;
        }

        private string CalcAuth(HttpMethod method, Uri url)
        {
            var time = (DateTime.Now - new DateTime(1970, 1, 1));
            string path = String.Format("{0}{1}{2}{3}", url.Scheme,
                    "://", url.Authority, url.AbsolutePath);
            var valueToHash = string.Format("{0} {1} {2} {3}", method.Method, path, (long)time.TotalMilliseconds,
                _apiSecret);
            var hash = CalculateSha1(valueToHash);
            var data = string.Format("{0} {1} {2}", method.Method, path, (long)time.TotalMilliseconds);
            return string.Format("apiKey=\"{0}\" data=\"{1}\" sig=\"{2}\"", _apiKey, data, hash);
        }

        private static string CalculateSha1(string value)
        {
            return Hash.CalculateSHA1(value);
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("SprdAuth", CalcAuth(request.Method, request.RequestUri));
            return base.SendAsync(request, cancellationToken);
        }
    }
}
