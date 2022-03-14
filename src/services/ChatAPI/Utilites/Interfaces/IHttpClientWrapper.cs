using System.Net.Http;
using System.Threading.Tasks;

namespace ChatAPI.Utilites.Interfaces
{
    public interface IHttpClientWrapper
    {
        Task<HttpResponseMessage> GetAsync(string url);
    }
}
