using System;
using System.Threading.Tasks;
using Checkout.RecruitmentTest.API.Client.Requests;
using Checkout.RecruitmentTest.API.Client.Responses;
using Checkout.RecruitmentTest.API.DTOs.Responses;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using TestStack.BDDfy;
using Xunit;

namespace Checkout.RecruitmentTest.API.Client.AcceptanceTests.UpdateBasketItem
{
    public class UpdateBasketItemTests
    {
        private CheckoutApiClient _client;
        private Guid _basketId;

        private AddBasketItemRequest _basketItem1 = new AddBasketItemRequest
        {
            Quantity = 2,
            Name = "Banana",
            Ref = "ABC",
            Price = 3.99M
        };

        private Guid _item2Id;
        private AddBasketItemRequest _basketItem2 = new AddBasketItemRequest
        {
            Quantity = 3,
            Name = "Apple",
            Ref = "XYZ",
            Price = 2.99M
        };

        private Guid _basketItem1Id;
        private Guid _basketItem2Id;
        private UpdateBasketItemRequest _updateBasketItem1Request = new UpdateBasketItemRequest { Quantity = 6 };
        private GetBasketItemsResponse _getBasketResponse;

        public UpdateBasketItemTests()
        {
            var factory = new WebApplicationFactory<Startup>();
            var httpClient = factory.CreateClient();
            _client = new CheckoutApiClient(httpClient);
        }

        [Fact]
        public void It_Updates_Basket_Item()
        {
            this.Given(x => x.AnExistingBasket())
                .And(x => x.TheBasketHasTwoBasketItems())
                .When(x => x.IUpdateItem1())
                .And(x => x.IGetTheBasket())
                .Then(x => x.ItReturnsABasketWithItem1Updated())
                .BDDfy();
        }

        private async Task AnExistingBasket()
        {
            _basketId = await _client.CreateBasketAsync();
        }

        private async Task TheBasketHasTwoBasketItems()
        {
            _basketItem1Id = await _client.AddBasketItemAsync(_basketId, _basketItem1);
            _basketItem2Id = await _client.AddBasketItemAsync(_basketId, _basketItem2);
        }

        private async Task IUpdateItem1()
        {
            await _client.UpdateBasketItemAsync(_basketId, _basketItem1Id, _updateBasketItem1Request);
        }

        private async Task IGetTheBasket()
        {
            _getBasketResponse = await _client.GetBasketItemsAsync(_basketId);
        }

        private void ItReturnsABasketWithItem1Updated()
        {
            _getBasketResponse.BasketItems.Count.Should().Be(2);

            _getBasketResponse.BasketItems.Should().Contain(x =>
                x.Id == _basketItem1Id
                && x.Name == _basketItem1.Name
                && x.Price == _basketItem1.Price
                && x.Ref == _basketItem1.Ref
                && x.Quantity == _updateBasketItem1Request.Quantity
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
