using System;
using System.Threading.Tasks;
using Checkout.RecruitmentTest.API.Client.Requests;
using Checkout.RecruitmentTest.API.Client.Responses;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using TestStack.BDDfy;
using Xunit;

namespace Checkout.RecruitmentTest.API.Client.AcceptanceTests.RemoveBasketItem
{
    public class RemoveBasketItemTests
    {
        private CheckoutApiClient _client;

        private Guid _item1Id;
        private AddBasketItemRequest _item1 = new AddBasketItemRequest
        {
            Quantity = 2,
            Name = "Banana",
            Ref = "ABC",
            Price = 3.99M
        };

        private Guid _item2Id;
        private AddBasketItemRequest _item2 = new AddBasketItemRequest
        {
            Quantity = 3,
            Name = "Apple",
            Ref = "XYZ",
            Price = 2.99M
        };

        private Guid _basketId;
        private GetBasketItemsResponse _response;

        public RemoveBasketItemTests()
        {
            var factory = new WebApplicationFactory<Startup>();
            var httpClient = factory.CreateClient();
            _client = new CheckoutApiClient(httpClient);
        }

        [Fact]
        public void It_Adds_BasketItem()
        {
            this.Given(x => x.AnExistingBasket())
                .And(x => x.TheBasketHasTwoItems())
                .When(x => x.IRemoveBasketItem1FromTheBasket())
                .And(x => x.IGetTheBasket())
                .Then(x => x.ItReturnsOneBasketItem())
                .BDDfy();
        }

        private async Task AnExistingBasket()
        {
            _basketId = await _client.CreateBasketAsync();
        }

        private async Task TheBasketHasTwoItems()
        {
            _item1Id = await _client.AddBasketItemAsync(_basketId, _item1);
            _item2Id = await _client.AddBasketItemAsync(_basketId, _item2);
        }

        private async Task IRemoveBasketItem1FromTheBasket()
        {
            await _client.RemoveBasketItemAsync(_basketId, _item1Id);
        }

        private async Task IGetTheBasket()
        {
            _response = await _client.GetBasketItemsAsync(_basketId);
        }

        private void ItReturnsOneBasketItem()
        {
            _response.BasketItems.Count.Should().Be(1);

            _response.BasketItems.Should().Contain(x =>
                x.Id == _item2Id
                && x.Name == _item2.Name
                && x.Price == _item2.Price
                && x.Ref == _item2.Ref
            );
        }
    }
}
