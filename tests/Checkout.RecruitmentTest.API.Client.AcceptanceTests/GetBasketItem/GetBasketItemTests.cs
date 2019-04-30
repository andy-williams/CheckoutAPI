using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Checkout.RecruitmentTest.API.Client.Responses;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using TestStack.BDDfy;
using Xunit;

namespace Checkout.RecruitmentTest.API.Client.AcceptanceTests.GetBasketItem
{
    public class GetBasketItemTests
    {
        private CheckoutApiClient _client;
        private Guid _basketId;
        private GetBasketItemsResponse _response;

        public GetBasketItemTests()
        {
            var factory = new WebApplicationFactory<Startup>();
            var httpClient = factory.CreateClient();
            _client = new CheckoutApiClient(httpClient);
        }

        [Fact]
        public void It_Gets_Empty_Basket()
        {
            this.Given(x => x.AnExistingBasket())
                .When(x => x.IGetTheBasket())
                .Then(x => x.ItReturnsAnEmptyBasket())
                .BDDfy();
        }

        private async Task AnExistingBasket()
        {
            _basketId = await _client.CreateBasketAsync();
        }

        private async Task IGetTheBasket()
        {
            _response = await _client.GetBasketItemsAsync(_basketId);
        }

        private void ItReturnsAnEmptyBasket()
        {
            _response.BasketItems.Count.Should().Be(0);
        }
    }
}
