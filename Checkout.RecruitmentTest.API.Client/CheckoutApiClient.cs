using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Checkout.RecruitmentTest.API.Client.Internal;
using Checkout.RecruitmentTest.API.Client.Requests;
using Checkout.RecruitmentTest.API.Client.Responses;
using Newtonsoft.Json;

namespace Checkout.RecruitmentTest.API.Client
{
    public interface ICheckoutApiClient
    {
        Task<Guid> CreateBasketAsync();
        Task<Guid> AddBasketItemAsync(Guid basketId, AddBasketItemRequest addBasketItemRequest);
        Task UpdateBasketItemAsync(Guid basketId, Guid basketItemId, UpdateBasketItemRequest updateBasketItemRequest);
        Task<GetBasketItemsResponse> GetBasketItemsAsync(Guid basketId);
        Task RemoveBasketItemAsync(Guid basketId, Guid basketItemId);
    }

    public class CheckoutApiClient : ICheckoutApiClient
    {
        private readonly HttpClient _httpClient;

        public CheckoutApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;

            if (_httpClient.BaseAddress == null)
                _httpClient.BaseAddress = new Uri("http://localhost:5000");
        }

        public async Task<Guid> CreateBasketAsync()
        {
            return (await PostAsync<BasketCreatedInternalResponse>("/basket")).BasketId;
        }

        public async Task<Guid> AddBasketItemAsync(Guid basketId, AddBasketItemRequest addBasketItemRequest)
        {
            if(addBasketItemRequest == null)
                throw new ArgumentNullException(nameof(addBasketItemRequest));

            return (await PostAsync<AddBasketItemInternalResponse>($"/basket/{basketId}", new AddBasketItemInternalRequest
            {
                Quantity = addBasketItemRequest.Quantity,
                Ref = addBasketItemRequest.Ref,
                Name = addBasketItemRequest.Name,
                Price = addBasketItemRequest.Price
            })).BasketItemId;
        }

        public async Task UpdateBasketItemAsync(Guid basketId, Guid basketItemId, UpdateBasketItemRequest updateBasketItemRequest)
        {
            if (updateBasketItemRequest == null)
                throw new ArgumentNullException(nameof(updateBasketItemRequest));

            var result = await PutAsync($"/basket/{basketId}/{basketItemId}", new UpdateBasketItemInternalRequest { Quantity = updateBasketItemRequest.Quantity });
            result.EnsureSuccessStatusCode();
        }

        public async Task<GetBasketItemsResponse> GetBasketItemsAsync(Guid basketId)
        {
            return await GetAsync<GetBasketItemsResponse>($"/basket/{basketId}");
        }

        public async Task RemoveBasketItemAsync(Guid basketId, Guid basketItemId)
        {
            var result = await DeleteAsync($"/basket/{basketId}/{basketItemId}");
            result.EnsureSuccessStatusCode();
        }

        private async Task<T> PostAsync<T>(string uri, object body)
        {
            var result = await _httpClient.PostAsync(uri, new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json"));
            result.EnsureSuccessStatusCode();

            return JsonConvert.DeserializeObject<T>(await result.Content.ReadAsStringAsync());
        }

        private async Task<T> PostAsync<T>(string uri)
        {
            var result = await _httpClient.PostAsync(uri, null);
            result.EnsureSuccessStatusCode();

            return JsonConvert.DeserializeObject<T>(await result.Content.ReadAsStringAsync());
        }

        private async Task<HttpResponseMessage> PutAsync(string uri, object body)
        {
            return await _httpClient.PutAsync(uri, new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json"));
        }

        private async Task<T> GetAsync<T>(string uri)
        {
            var result = await _httpClient.GetAsync(uri);
            result.EnsureSuccessStatusCode();

            return JsonConvert.DeserializeObject<T>(await result.Content.ReadAsStringAsync());
        }

        private async Task<HttpResponseMessage> DeleteAsync(string uri)
        {
            return await _httpClient.DeleteAsync(uri);
        }
    }
}