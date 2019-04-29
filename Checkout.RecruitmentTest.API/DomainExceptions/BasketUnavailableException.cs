using System;

namespace Checkout.RecruitmentTest.API.DomainExceptions
{
    public class BasketUnavailableException : DomainException
    {
        public BasketUnavailableException(Guid basketId) : base($"Basket {basketId.ToString()} does not exist") { }
    }
}