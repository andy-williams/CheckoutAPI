using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Checkout.RecruitmentTest.API.Services;
using MediatR;

namespace Checkout.RecruitmentTest.API.Handlers.Queries
{
    public class GetBasketItemsQuery : IRequest<IList<BasketItem>>
    {
        public Guid BasketId { get; set; }
    }

    public class GetBasketItemsQueryHandler : IRequestHandler<GetBasketItemsQuery, IList<BasketItem>>
    {
        private readonly IBasketDataStore _basketDataStore;

        public GetBasketItemsQueryHandler(IBasketDataStore basketDataStore)
        {
            _basketDataStore = basketDataStore;
        }

        public Task<IList<BasketItem>> Handle(GetBasketItemsQuery request, CancellationToken cancellationToken)
        {
            return Task.FromResult(_basketDataStore.GetBasketItems(request.BasketId));
        }
    }
}