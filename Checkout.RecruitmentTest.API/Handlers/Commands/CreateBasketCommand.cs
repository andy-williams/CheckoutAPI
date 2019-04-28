using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Checkout.RecruitmentTest.API.Data;
using MediatR;

namespace Checkout.RecruitmentTest.API.Handlers.Commands
{
    public class CreateBasketCommand : IRequest<Guid>  {  }

    public class CreateBasketCommandHandler : IRequestHandler<CreateBasketCommand, Guid>
    {
        private readonly IDictionary<Guid, IList<BasketItem>> _basketDataStore;

        public CreateBasketCommandHandler(IDictionary<Guid, IList<BasketItem>> basketDataStore)
        {
            _basketDataStore = basketDataStore;
        }

        public Task<Guid> Handle(CreateBasketCommand request, CancellationToken cancellationToken)
        {
            var newBasketId = Guid.NewGuid();
            _basketDataStore.Add(newBasketId, new List<BasketItem>());

            return Task.FromResult(newBasketId);
        }
    }
}
