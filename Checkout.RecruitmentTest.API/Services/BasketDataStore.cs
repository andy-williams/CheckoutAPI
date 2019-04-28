using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Checkout.RecruitmentTest.API.Handlers.Queries;

namespace Checkout.RecruitmentTest.API.Services
{
    public class BasketDataStore : IBasketDataStore
    {
        private readonly IDictionary<Guid, IList<BasketItem>> _baskets;

        public BasketDataStore()
        {
            _baskets = new ConcurrentDictionary<Guid, IList<BasketItem>>();
        }

        public Guid CreateBasket()
        {
            var newBasketId = Guid.NewGuid();
            _baskets.Add(newBasketId, new List<BasketItem>());
            return newBasketId;
        }

        public IList<BasketItem> GetBasketItems(Guid basketId)
        {
            return _baskets[basketId];
        }

        public Guid AddBasketItem(Guid basketId, BasketItem basketItem)
        {
            var basketItemId = Guid.NewGuid();
            basketItem.Id = basketItemId;
            _baskets[basketId].Add(basketItem);

            return basketItemId;
        }

        public void UpdateBasketItem(Guid basketId, BasketItem updateBasketItem)
        {
            throw new NotImplementedException();
        }

        public void RemoveBasketItem(Guid basketId, Guid basketItemId)
        {
            throw new NotImplementedException();
        }
    }

    public interface IBasketDataStore
    {
        Guid CreateBasket();
        IList<BasketItem> GetBasketItems(Guid requestBasketId);
        Guid AddBasketItem(Guid basketId, BasketItem addBasketItem);
        void UpdateBasketItem(Guid basketId, BasketItem updateBasketItem);
        void RemoveBasketItem(Guid basketId, Guid basketItemId);
    }

    public class BasketItem
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
