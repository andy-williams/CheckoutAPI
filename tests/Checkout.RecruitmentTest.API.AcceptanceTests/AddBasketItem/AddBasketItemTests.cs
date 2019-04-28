using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Checkout.RecruitmentTest.API.AcceptanceTests.Infrastructure;
using Checkout.RecruitmentTest.API.DTOs.Requests;
using Checkout.RecruitmentTest.API.DTOs.Responses;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using TestStack.BDDfy;
using Xunit;

namespace Checkout.RecruitmentTest.API.AcceptanceTests.AddBasketItem
{
    public class AddBasketItemTests
    {
        private CheckoutHttpClient _client;
        private Guid _basketId;
        private GetBasketResponse _getBasketResponse;

        private AddBasketItemRequest _basketItem1;
        private AddBasketItemRequest _basketItem2;
        private Guid _basketItem1Id;
        private Guid _basketItem2Id;

        public AddBasketItemTests()
        {
            var factory = new WebApplicationFactory<Startup>();
            _client = new CheckoutHttpClient(factory.CreateClient());

            _basketItem1 = new AddBasketItemRequest
            {
                Quantity = 2,
                Ref = "ABC",
                Name = "Banana",
                Price = 2.99M,
            };

            _basketItem2 = new AddBasketItemRequest
            {
                Quantity = 1,
                Ref = "XYZ",
                Name = "Apple",
                Price = 4.99M,
            };
        }

        [Fact]
        public void It_Adds_Basket_Items()
        {
            this.Given(x => x.AnExistingBasket())
                .And(x => x.IAddTwoBasketItems())
                .When(x => x.IGetTheBasket())
                .Then(x => x.ItReturnsABasketWithTwoItems())
                .BDDfy();
        }

        private async Task AnExistingBasket()
        {
            _basketId = (await _client.PostAsync<BasketCreatedResponse>("/basket")).BasketId;
        }

        private async Task IAddTwoBasketItems()
        {
            _basketItem1Id = (await _client.PostAsync<AddBasketItemResponse>($"/basket/{_basketId}", _basketItem1)).BasketItemId;
            _basketItem2Id = (await _client.PostAsync<AddBasketItemResponse>($"/basket/{_basketId}",_basketItem2)).BasketItemId;
        }

        private async Task IGetTheBasket()
        {
            _getBasketResponse = await _client.GetAsync<GetBasketResponse>($"/basket/{_basketId}");
        }

        private void ItReturnsABasketWithTwoItems()
        {
            _getBasketResponse.BasketItems.Count.Should().Be(2);

            _getBasketResponse.BasketItems.Should().Contain(x =>
                x.Id == _basketItem1Id
                && x.Name == _basketItem1.Name
                && x.Price == _basketItem1.Price
                && x.Ref == _basketItem1.Ref
                && x.Quantity == _basketItem1.Quantity
            );
            _getBasketResponse.BasketItems.Should().Contain(x =>
                x.Id == _basketItem2Id
                && x.Name == _basketItem2.Name
                && x.Price == _basketItem2.Price
                && x.Ref == _basketItem2.Ref
                && x.Quantity == _basketItem2.Quantity
            );
        }
    }
}
