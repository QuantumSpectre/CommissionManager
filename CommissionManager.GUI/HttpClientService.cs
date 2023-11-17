using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;

namespace CommissionManager.GUI
{
    public class HttpClientService
    {
        private readonly HttpClient _httpClient;

        public HttpClientService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<HttpResponseMessage> PostAsync(string url, object data)
        {
            using (var content = new ObjectContent<object>(data, new JsonMediaTypeFormatter()))
            {
                return await _httpClient.PostAsync(url, content);
            }
        }

        public void Dispose()
        {
            _httpClient.Dispose();
        }
    }

}
