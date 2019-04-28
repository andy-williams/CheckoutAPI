using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Checkout.RecruitmentTest.API.DTOs.Requests;
using Newtonsoft.Json;

namespace Checkout.RecruitmentTest.API.AcceptanceTests
{
    public class CheckoutHttpClient
    {
        private readonly HttpClient _httpClient;

        public CheckoutHttpClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<T> PostAsync<T>(string uri)
        {
            var result = await _httpClient.PostAsync(uri, null);
            result.EnsureSuccessStatusCode();

            return JsonConvert.DeserializeObject<T>(await result.Content.ReadAsStringAsync());
        }

        public async Task<T> PostAsync<T>(string uri, object body)
        {
            var result = await _httpClient.PostAsync(uri, new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json"));
            result.EnsureSuccessStatusCode();

            return JsonConvert.DeserializeObject<T>(await result.Content.ReadAsStringAsync());
        }

        public async Task<T> GetAsync<T>(string uri)
        {
            var result = await _httpClient.GetAsync(uri);
            result.EnsureSuccessStatusCode();

            return JsonConvert.DeserializeObject<T>(await result.Content.ReadAsStringAsync());
        }

        public async Task<HttpResponseMessage> PutAsync(string uri, object body)
        {
            return await _httpClient.PutAsync(uri, new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json"));
        }

        public async Task<HttpResponseMessage> DeleteAsync(string uri)
        {
            return await _httpClient.DeleteAsync(uri);
        }
    }
}
