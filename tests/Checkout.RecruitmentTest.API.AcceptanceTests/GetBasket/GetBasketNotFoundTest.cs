using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Checkout.RecruitmentTest.API.AcceptanceTests.Infrastructure;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using TestStack.BDDfy;
using Xunit;

namespace Checkout.RecruitmentTest.API.AcceptanceTests.GetBasket
{
    public class GetBasketNotFoundTest
    {
        private CheckoutHttpClient _client;
        private HttpResponseMessage _response;

        public GetBasketNotFoundTest()
        {
            var factory = new WebApplicationFactory<Startup>();
            _client = new CheckoutHttpClient(factory.CreateClient());
        }

        [Fact]
        public void It_Returns_Not_Found()
        {
            this.When(x => x.ITryToGetANonExistantBasket())
                .Then(x => x.ItReturnsNotFound())
                .BDDfy();
        }

        private async Task ITryToGetANonExistantBasket()
        {
            _response = await _client.GetAsync($"/basket/{Guid.NewGuid()}");
        }

        private void ItReturnsNotFound()
        {
            _response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}
