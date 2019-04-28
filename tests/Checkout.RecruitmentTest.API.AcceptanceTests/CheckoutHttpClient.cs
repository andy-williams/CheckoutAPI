using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
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
    }
}
