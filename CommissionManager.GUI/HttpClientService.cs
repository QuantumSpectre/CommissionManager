using System;
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

        public async Task<HttpResponseMessage> GetAsync(string url)
        {
            return await _httpClient.GetAsync(url);
        }

        public async Task<HttpResponseMessage> PatchAsync(string url, object data)
        {
            var request = new HttpRequestMessage(new HttpMethod("PATCH"), url);

            using (var content = new ObjectContent<object>(data, new JsonMediaTypeFormatter()))
            {
                request.Content = content;
                return await _httpClient.SendAsync(request);
            }
        }

        public async Task<bool> DeleteAsync(string url)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Delete, url);

                var response = await _httpClient.SendAsync(request);

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting: {ex.Message}");

                return false;             }
        }

        public void Dispose()
        {
            _httpClient.Dispose();
        }
    }

}
