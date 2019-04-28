using System;
using System.Threading;
using System.Threading.Tasks;
using Checkout.RecruitmentTest.API.Services;
using MediatR;

namespace Checkout.RecruitmentTest.API.Controllers
{
    public class RemoveBasketItemCommand : IRequest
    {
        public Guid BasketId { get; set; }
        public Guid BasketItemId { get; set; }
    }

    public class RemoveBasketItemCommandHandler : IRequestHandler<RemoveBasketItemCommand>
    {
        private readonly IBasketDataStore _dataStore;

        public RemoveBasketItemCommandHandler(IBasketDataStore dataStore)
        {
            _dataStore = dataStore;
        }

        public async Task<Unit> Handle(RemoveBasketItemCommand request, CancellationToken cancellationToken)
        {
            _dataStore.RemoveBasketItem(request.BasketId, request.BasketItemId);
            return await Unit.Task;
        }
    }

}