using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Checkout.RecruitmentTest.API.AcceptanceTests.Infrastructure;
using Checkout.RecruitmentTest.API.DTOs.Responses;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using TestStack.BDDfy;
using Xunit;

namespace Checkout.RecruitmentTest.API.AcceptanceTests.RemoveBasketItem
{
    public class RemoveBasketItemNotFoundTests
    {
        private CheckoutHttpClient _client;
        private HttpResponseMessage _response;
        private Guid _basketId;

        public RemoveBasketItemNotFoundTests()
        {
            var factory = new WebApplicationFactory<Startup>();
            _client = new CheckoutHttpClient(factory.CreateClient());
        }

        [Fact]
        public void It_Returns_Not_Found()
        {
            this.Given(x => x.AnExistingBasket())
                .When(x => x.ITryToRemoveANonExistantBasketItem())
                .Then(x => x.ItReturnsNotFound())
                .BDDfy();
        }

        private async Task AnExistingBasket()
        {
            _basketId = (await _client.PostAsync<CreateBasketResponse>("/basket")).BasketId;
        }

        private async Task ITryToRemoveANonExistantBasketItem()
        {
            _response = await _client.DeleteAsync($"/basket/{_basketId}/{Guid.NewGuid()}");
        }

        private void ItReturnsNotFound()
        {
            _response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}
