using System;
using System.Threading.Tasks;
using Checkout.RecruitmentTest.API.Client.Requests;
using Checkout.RecruitmentTest.API.Client.Responses;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using TestStack.BDDfy;
using Xunit;

namespace Checkout.RecruitmentTest.API.Client.AcceptanceTests.AddBasketItem
{
    public class AddBasketItemTests
    {
        private CheckoutApiClient _client;
        private Guid _basketId;

        private AddBasketItemRequest _item = new AddBasketItemRequest
        {
            Quantity = 2,
            Name = "Banana",
            Ref = "ABC",
            Price = 3.99M
        };
        private GetBasketItemsResponse _response;
        private Guid _basketItemId;

        public AddBasketItemTests()
        {
            var factory = new WebApplicationFactory<Startup>();
            var httpClient = factory.CreateClient();
            _client = new CheckoutApiClient(httpClient);
        }

        [Fact]
        public void It_Adds_BasketItem()
        {
            this.Given(x => x.AnExistingBasket())
                .When(x => x.IAddABasketItem())
                .And(x => x.IGetTheBasket())
                .Then(x => x.BasketItemExistsInTheBasket())
                .BDDfy();
        }

        private async Task AnExistingBasket()
        {
            _basketId = await _client.CreateBasketAsync();
        }

        private async Task IAddABasketItem()
        {
            _basketItemId = await _client.AddBasketItemAsync(_basketId, _item);
        }

        private async Task IGetTheBasket()
        {
            _response = await _client.GetBasketItemsAsync(_basketId);
        }

        private void BasketItemExistsInTheBasket()
        {
            _response.BasketItems.Count.Should().Be(1);

            _response.BasketItems.Should().Contain(x =>
                x.Id == _basketItemId
                && x.Quantity == _item.Quantity
                && x.Name == _item.Name
                && x.Ref == _item.Ref
                && x.Price == _item.Price
            );
        }
    }
}
