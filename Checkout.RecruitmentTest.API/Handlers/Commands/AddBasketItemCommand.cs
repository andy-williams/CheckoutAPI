using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Checkout.RecruitmentTest.API.Data;
using MediatR;

namespace Checkout.RecruitmentTest.API.Handlers.Commands
{
    public class AddBasketItemCommand : IRequest<Guid>
    {
        public Guid BasketId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }

    public class AddBasketItemCommandHandler : IRequestHandler<AddBasketItemCommand, Guid>
    {
        private readonly IDictionary<Guid, IList<BasketItem>> _basketDataStore;

        public AddBasketItemCommandHandler(IDictionary<Guid, IList<BasketItem>> basketDataStore)
        {
            _basketDataStore = basketDataStore;
        }

        public async Task<Guid> Handle(AddBasketItemCommand request, CancellationToken cancellationToken)
        {
            var basketItemId = Guid.NewGuid();
            _basketDataStore[request.BasketId].Add(new BasketItem
            {
                Id = basketItemId,
                Name = request.Name,
                Price = request.Price,
                Quantity = request.Quantity
            });
            return await Task.FromResult(basketItemId);
        }
    }
}