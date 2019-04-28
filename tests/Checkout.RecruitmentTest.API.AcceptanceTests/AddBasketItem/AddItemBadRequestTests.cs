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

namespace Checkout.RecruitmentTest.API.AcceptanceTests.AddBasketItem
{
    public class AddItemBadRequestTests
    {
        private CheckoutHttpClient _client;

        private Guid _basketId;
        private AddBasketItemRequest _basketItem;

        private HttpResponseMessage _addBasketItemResponse;

        public AddItemBadRequestTests()
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
        public void It_Returns_BadRequest_When_Adding_Two_Items_Of_The_Same_Ref()
        {
            this.Given(x => x.AnExistingBasket())
                .And(x => x.TheBasketHasAnItem())
                .When(x => x.ITryToAddAnotherItemWithTheSameRef())
                .Then(x => x.ItReturnsBadRequest())
                .BDDfy();
        }

        [Theory]
        [InlineData(0)]
        [InlineData(0.001)]
        [InlineData(-1)]
        public async Task It_Returns_BadRequest_When_Price_Is_Invalid(decimal price)
        {
            var addBasketItemRequest = new BadAddBasketItemRequest
            {
                Name = Guid.NewGuid().ToString(),
                Ref = Guid.NewGuid().ToString(),
                Quantity = 1,
                Price = price
            };

            var addBasketItemResponse = await _client.PostAsync($"/basket/{_basketId}", addBasketItemRequest);

            addBasketItemResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task It_Returns_BadRequest_When_Price_Is_Null()
        {
            var addBasketItemRequest = new BadAddBasketItemRequest
            {
                Name = Guid.NewGuid().ToString(),
                Ref = Guid.NewGuid().ToString(),
                Quantity = 1,
                Price = null
            };

            var addBasketItemResponse = await _client.PostAsync($"/basket/{_basketId}", addBasketItemRequest);

            addBasketItemResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public async Task It_Returns_BadRequest_When_Ref_Is_Invalid(string @ref)
        {
            var addBasketItemRequest = new BadAddBasketItemRequest
            {
                Name = Guid.NewGuid().ToString(),
                Ref = @ref,
                Quantity = 1,
                Price = 1.99M
            };

            var addBasketItemResponse = await _client.PostAsync($"/basket/{_basketId}", addBasketItemRequest);

            addBasketItemResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public async Task It_Returns_BadRequest_When_Name_Is_Invalid(string name)
        {
            var addBasketItemRequest = new BadAddBasketItemRequest
            {
                Name = name,
                Ref = Guid.NewGuid().ToString(),
                Quantity = 1,
                Price = 1.99M
            };

            var addBasketItemResponse = await _client.PostAsync($"/basket/{_basketId}", addBasketItemRequest);

            addBasketItemResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task It_Returns_BadRequest_When_Quantity_Is_Invalid(int quantity)
        {
            var addBasketItemRequest = new BadAddBasketItemRequest
            {
                Name = Guid.NewGuid().ToString(),
                Ref = Guid.NewGuid().ToString(),
                Quantity = quantity,
                Price = 1.99M
            };

            var addBasketItemResponse = await _client.PostAsync($"/basket/{_basketId}", addBasketItemRequest);

            addBasketItemResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task It_Returns_BadRequest_When_Quantity_Is_Null()
        {
            var addBasketItemRequest = new BadAddBasketItemRequest
            {
                Name = Guid.NewGuid().ToString(),
                Ref = Guid.NewGuid().ToString(),
                Quantity = null,
                Price = 3.99M
            };

            var addBasketItemResponse = await _client.PostAsync($"/basket/{_basketId}", addBasketItemRequest);

            addBasketItemResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }


        private async Task AnExistingBasket()
        {
            _basketId = (await _client.PostAsync<BasketCreatedResponse>("/basket")).BasketId;
        }

        private async Task TheBasketHasAnItem()
        {
            await _client.PostAsync<AddBasketItemResponse>($"/basket/{_basketId}", _basketItem);
        }

        private async Task ITryToAddAnotherItemWithTheSameRef()
        {
            _addBasketItemResponse = await _client.PostAsync($"/basket/{_basketId}", _basketItem);
        }

        private void ItReturnsBadRequest()
        {
            _addBasketItemResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}
