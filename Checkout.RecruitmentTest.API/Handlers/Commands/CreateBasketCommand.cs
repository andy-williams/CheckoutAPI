using System;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using Checkout.RecruitmentTest.API.Services;
using MediatR;

namespace Checkout.RecruitmentTest.API.Handlers.Commands
{
    public class CreateBasketCommand : IRequest<Guid>  {  }

    public class CreateBasketCommandHandler : IRequestHandler<CreateBasketCommand, Guid>
    {
        private readonly IBasketDataStore _basketDataStore;

        public CreateBasketCommandHandler(IBasketDataStore basketDataStore)
        {
            _basketDataStore = basketDataStore;
        }

        public Task<Guid> Handle(CreateBasketCommand request, CancellationToken cancellationToken)
        {
            return Task.FromResult(_basketDataStore.CreateBasket());
        }
    }
}
