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
    public class RemoveBasketItemCommand : IRequest
    {
        public Guid BasketId { get; set; }
        public Guid BasketItemId { get; set; }
    }

    public class RemoveBasketItemCommandHandler : IRequestHandler<RemoveBasketItemCommand>
    {
        private readonly IDictionary<Guid, IList<BasketItem>> _basketDataStore;

        public RemoveBasketItemCommandHandler(IDictionary<Guid, IList<BasketItem>> basketDataStore)
        {
            _basketDataStore = basketDataStore;
        }

        public async Task<Unit> Handle(RemoveBasketItemCommand request, CancellationToken cancellationToken)
        {
            if(!_basketDataStore.ContainsKey(request.BasketId))
                throw new BasketUnavailableException(request.BasketId);

            var basket = _basketDataStore[request.BasketId];
            var basketItem = basket.FirstOrDefault(x => x.Id == request.BasketItemId);

            if (basketItem == null)
                throw new BasketItemUnavailableException(request.BasketId, request.BasketItemId);

            _basketDataStore[request.BasketId].Remove(basketItem);
            return await Unit.Task;
        }
    }
}