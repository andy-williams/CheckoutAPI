using System;
using System.Threading.Tasks;
using Checkout.RecruitmentTest.API.AcceptanceTests.Infrastructure;
using Checkout.RecruitmentTest.API.DTOs.Responses;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using TestStack.BDDfy;
using Xunit;

namespace Checkout.RecruitmentTest.API.AcceptanceTests.GetBasket
{
    public class GetBasketTests
    {
        private CheckoutHttpClient _client;
        private Guid _basketId;
        private GetBasketResponse _getBasketResponse;

        public GetBasketTests()
        {
            var factory = new WebApplicationFactory<Startup>();
            _client = new CheckoutHttpClient(factory.CreateClient());
        }

        [Fact]
        public void It_Gets_Empty_Basket()
        {
            this.Given(x => x.AnExistingBasket())
                .When(x => x.IGetTheBasket())
                .Then(x => x.ItShouldReturnAnEmptyBasket())
                .BDDfy();
        }

        private async Task AnExistingBasket()
        {
            _basketId = (await _client.PostAsync<BasketCreatedResponse>("/basket", null)).BasketId;
        }


        private async Task IGetTheBasket()
        {
            _getBasketResponse = await _client.GetAsync<GetBasketResponse>($"/basket/{_basketId}");
        }

        private void ItShouldReturnAnEmptyBasket()
        {
            _getBasketResponse.BasketItems.Count.Should().Be(0);
        }
    }
}
