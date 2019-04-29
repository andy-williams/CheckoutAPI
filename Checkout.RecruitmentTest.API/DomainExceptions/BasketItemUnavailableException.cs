using System;

namespace Checkout.RecruitmentTest.API.DomainExceptions
{
    public class BasketItemUnavailableException : DomainException
    {
        public BasketItemUnavailableException(Guid basketId, Guid basketItemId) : base($"Basket item {basketItemId.ToString()} not found in basket {basketId}") { }
    }
}