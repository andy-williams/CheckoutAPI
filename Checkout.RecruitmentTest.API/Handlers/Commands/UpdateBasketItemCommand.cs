using System;
using System.Threading;
using System.Threading.Tasks;
using Checkout.RecruitmentTest.API.Services;
using MediatR;

namespace Checkout.RecruitmentTest.API.Handlers.Commands
{
    public class UpdateBasketItemCommand : IRequest
    {
        public Guid BasketId { get; set; }
        public Guid BasketItemId { get; set; }
        public int Quantity { get; set; }
    }

    public class UpdateBasketItemCommandHandler : IRequestHandler<UpdateBasketItemCommand>
    {
        private readonly IBasketDataStore _basketDataStore;

        public UpdateBasketItemCommandHandler(IBasketDataStore basketDataStore)
        {
            _basketDataStore = basketDataStore;
        }

        public async Task<Unit> Handle(UpdateBasketItemCommand request, CancellationToken cancellationToken)
        {
            _basketDataStore.UpdateBasketItem(request.BasketId, new BasketItem
            {
                Id = request.BasketItemId,
                Quantity = request.Quantity
            });


            return await Unit.Task;
        }
    }
}