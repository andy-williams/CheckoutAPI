using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Checkout.RecruitmentTest.API.Data;

namespace Checkout.RecruitmentTest.API.Controllers
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
            var basketItem = _basketDataStore[request.BasketId].First(x => x.Id == request.BasketItemId);
            _basketDataStore[request.BasketId].Remove(basketItem);
            return await Unit.Task;
        }
    }

}