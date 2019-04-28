using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Checkout.RecruitmentTest.API.Data;
using MediatR;

namespace Checkout.RecruitmentTest.API.Handlers.Queries
{
    public class GetBasketItemsQuery : IRequest<IList<BasketItem>>
    {
        public Guid BasketId { get; set; }
    }

    public class GetBasketItemsQueryHandler : IRequestHandler<GetBasketItemsQuery, IList<BasketItem>>
    {
        private readonly IDictionary<Guid, IList<BasketItem>> _basketDataStore;

        public GetBasketItemsQueryHandler(IDictionary<Guid, IList<BasketItem>> basketDataStore)
        {
            _basketDataStore = basketDataStore;
        }

        public Task<IList<BasketItem>> Handle(GetBasketItemsQuery request, CancellationToken cancellationToken)
        {
            return Task.FromResult(_basketDataStore[request.BasketId]);
        }
    }
}