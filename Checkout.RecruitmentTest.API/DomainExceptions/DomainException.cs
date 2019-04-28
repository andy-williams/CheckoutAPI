using System;

namespace Checkout.RecruitmentTest.API.DomainExceptions
{
    public class DomainException : Exception
    {
        public DomainException() : base() { }
        public DomainException(string message) : base(message) { }
    }

    public class DuplicateBasketItemException : DomainException
    {
        public DuplicateBasketItemException(string itemRef) : base($"{itemRef} already in basket") { }
    }
}
