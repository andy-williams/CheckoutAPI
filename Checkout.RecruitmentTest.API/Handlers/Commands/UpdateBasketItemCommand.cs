using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Checkout.RecruitmentTest.API.Data;
using Checkout.RecruitmentTest.API.DomainExceptions;
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
        private readonly IDictionary<Guid, IList<BasketItem>> _basketDataStore;

        public UpdateBasketItemCommandHandler(IDictionary<Guid, IList<BasketItem>> basketDataStore)
        {
            _basketDataStore = basketDataStore;
        }

        public async Task<Unit> Handle(UpdateBasketItemCommand request, CancellationToken cancellationToken)
        {
            if(!_basketDataStore.ContainsKey(request.BasketId))
                throw new BasketUnavailableException(request.BasketId);

            var basket = _basketDataStore[request.BasketId];
            var basketItem = basket.FirstOrDefault(x => x.Id == request.BasketItemId);

            if(basketItem == null)
                throw new BasketItemUnavailableException(request.BasketId, request.BasketItemId);

            basketItem.Quantity = request.Quantity;

            return await Unit.Task;
        }
    }
}