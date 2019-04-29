using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Checkout.RecruitmentTest.API.AcceptanceTests.Infrastructure;
using Checkout.RecruitmentTest.API.DTOs.Requests;
using Checkout.RecruitmentTest.API.DTOs.Responses;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using TestStack.BDDfy;
using Xunit;

namespace Checkout.RecruitmentTest.API.AcceptanceTests.UpdateBasketItem
{
    public class UpdateBasketItemNotFoundTests
    {
        private CheckoutHttpClient _client;
        private HttpResponseMessage _response;
        private Guid _basketId;

        public UpdateBasketItemNotFoundTests()
        {
            var factory = new WebApplicationFactory<Startup>();
            _client = new CheckoutHttpClient(factory.CreateClient());
        }

        [Fact]
        public void It_Returns_Not_Found()
        {
            this.Given(x => x.AnExistingBasket())
                .When(x => x.ITryToUpdateANonExistantBasket())
                .Then(x => x.ItReturnsNotFound())
                .BDDfy();
        }

        private async Task AnExistingBasket()
        {
            _basketId = (await _client.PostAsync<BasketCreatedResponse>("/basket")).BasketId;
        }

        private async Task ITryToUpdateANonExistantBasket()
        {
            _response = await _client.PutAsync($"/basket/{_basketId}/{Guid.NewGuid()}", new UpdateBasketItemRequest { Quantity = 2 });
        }

        private void ItReturnsNotFound()
        {
            _response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}
