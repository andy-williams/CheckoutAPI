using System;
using System.Threading;
using System.Threading.Tasks;
using Checkout.RecruitmentTest.API.Services;
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
        private readonly IBasketDataStore _basketDataStore;

        public AddBasketItemCommandHandler(IBasketDataStore basketDataStore)
        {
            _basketDataStore = basketDataStore;
        }

        public async Task<Guid> Handle(AddBasketItemCommand request, CancellationToken cancellationToken)
        {
            return await Task.FromResult(_basketDataStore.AddBasketItem(request.BasketId, new BasketItem
            {
                Name = request.Name,
                Price = request.Price,
                Quantity =  request.Quantity
            }));
        }
    }
}