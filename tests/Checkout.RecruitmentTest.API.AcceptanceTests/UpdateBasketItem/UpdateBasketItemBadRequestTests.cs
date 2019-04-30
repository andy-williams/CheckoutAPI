using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Checkout.RecruitmentTest.API.AcceptanceTests.Infrastructure;
using Checkout.RecruitmentTest.API.AcceptanceTests.Infrastructure.BadRequests;
using Checkout.RecruitmentTest.API.DTOs.Requests;
using Checkout.RecruitmentTest.API.DTOs.Responses;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using TestStack.BDDfy;
using Xunit;

namespace Checkout.RecruitmentTest.API.AcceptanceTests.UpdateBasketItem
{
    public class UpdateBasketItemBadRequestTests
    {
        private CheckoutHttpClient _client;
        private Guid _basketId;
        private AddBasketItemRequest _basketItem;
        private Guid _basketItemId;
        private HttpResponseMessage _response;

        public UpdateBasketItemBadRequestTests()
        {
            var factory = new WebApplicationFactory<Startup>();
            _client = new CheckoutHttpClient(factory.CreateClient());

            _basketItem = new AddBasketItemRequest
            {
                Quantity = 2,
                Ref = "ABC",
                Name = "Banana",
                Price = 2.99M,
            };
        }

        [Fact]
        public void It_Returns_BadRequest_When_Quantity_Is_Null()
        {
            this.Given(x => x.AnExistingBasket())
                .And(x => x.TheBasketHasAnItem())
                .When(x => x.ITryToUpdateTheBasketItemWithQuantityOfNull())
                .Then(x => x.ItReturnsBadRequest())
                .BDDfy();
        }        

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task It_Returns_BadRequest_When_Quantity_Is_Invalid(int quantity)
        {
            var basketItem = new AddBasketItemRequest
            {
                Quantity = 2,
                Ref = "ABC",
                Name = "Banana",
                Price = 2.99M,
            };
            var basketId = (await _client.PostAsync<CreateBasketResponse>("/basket")).BasketId;
            var basketItem1Id = (await _client.PostAsync<AddBasketItemResponse>($"/basket/{basketId}", basketItem)).BasketItemId;

            var response = await _client.PutAsync($"/basket/{basketId}/{basketItem1Id}", new UpdateBasketItemRequest { Quantity = quantity });

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        private async Task AnExistingBasket()
        {
            _basketId = (await _client.PostAsync<CreateBasketResponse>("/basket")).BasketId;
        }

        private async Task TheBasketHasAnItem()
        {
            _basketItemId = (await _client.PostAsync<AddBasketItemResponse>($"/basket/{_basketId}", _basketItem)).BasketItemId;
        }

        private async Task ITryToUpdateTheBasketItemWithQuantityOfNull()
        {
            _response = await _client.PutAsync($"/basket/{_basketId}/{_basketItemId}", new BadUpdateBasketItemRequest { Quantity = null });
        }

        private void ItReturnsBadRequest()
        {
            _response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}
