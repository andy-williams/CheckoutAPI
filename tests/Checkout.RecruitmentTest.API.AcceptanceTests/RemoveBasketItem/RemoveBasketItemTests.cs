using System;
using System.Threading.Tasks;
using Checkout.RecruitmentTest.API.AcceptanceTests.Infrastructure;
using Checkout.RecruitmentTest.API.DTOs.Requests;
using Checkout.RecruitmentTest.API.DTOs.Responses;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using TestStack.BDDfy;
using Xunit;

namespace Checkout.RecruitmentTest.API.AcceptanceTests.RemoveBasketItem
{
    public class RemoveBasketItemTests
    {
        private CheckoutHttpClient _client;
        private Guid _basketId;
        private GetBasketResponse _getBasketResponse;

        private AddBasketItemRequest _basketItem1;
        private AddBasketItemRequest _basketItem2;
        private Guid _basketItem1Id;
        private Guid _basketItem2Id;

        public RemoveBasketItemTests()
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
        public void It_Removes_Basket_Item()
        {
            this.Given(x => x.AnExistingBasket())
                .And(x => x.TheBasketHasTwoBasketItems())
                .When(x => x.IRemoveBasketItem1FromTheBasket())
                .And(x => x.IGetTheBasket())
                .Then(x => x.ItReturnsOneBasketItem())
                .BDDfy();
        }

        private async Task AnExistingBasket()
        {
            _basketId = (await _client.PostAsync<BasketCreatedResponse>("/basket")).BasketId;
        }

        private async Task TheBasketHasTwoBasketItems()
        {
            _basketItem1Id = (await _client.PostAsync<AddBasketItemResponse>($"/basket/{_basketId}", _basketItem1)).BasketItemId;
            _basketItem2Id = (await _client.PostAsync<AddBasketItemResponse>($"/basket/{_basketId}",_basketItem2)).BasketItemId;
        }

        private async Task IRemoveBasketItem1FromTheBasket()
        {
            var response = await _client.DeleteAsync($"/basket/{_basketId}/{_basketItem1Id}");
            response.EnsureSuccessStatusCode();
        }


        private async Task IGetTheBasket()
        {
            _getBasketResponse = await _client.GetAsync<GetBasketResponse>($"/basket/{_basketId}");
        }

        private void ItReturnsOneBasketItem()
        {
            _getBasketResponse.BasketItems.Count.Should().Be(1);

            _getBasketResponse.BasketItems.Should().Contain(x =>
                x.Id == _basketItem2Id
                && x.Name == _basketItem2.Name
                && x.Price == _basketItem2.Price
                && x.Quantity == _basketItem2.Quantity
            );
        }
    }
}
