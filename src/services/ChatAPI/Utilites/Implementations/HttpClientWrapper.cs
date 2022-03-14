using ChatAPI.Utilites.Interfaces;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Threading.Tasks;

namespace ChatAPI.Utilites.Implementations
{
    public class HttpClientWrapper : IHttpClientWrapper
    {
        private IHttpClientFactory clientFactory;
        private readonly ILogger<HttpClientWrapper> logger;
        private const string DefaultClientName = "DefaultClient";

        public HttpClientWrapper(IHttpClientFactory httpFactory, ILogger<HttpClientWrapper> logger)
        {
            this.clientFactory = httpFactory;
            this.logger = logger;
        }

        public async Task<HttpResponseMessage> GetAsync(string url)
        {
            var client = this.clientFactory.CreateClient(DefaultClientName);
            return await client.GetAsync(url);
        }
    }
}
