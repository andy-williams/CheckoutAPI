using System;

namespace Checkout.RecruitmentTest.API.DomainExceptions
{
    public class DomainException : Exception
    {
        public DomainException() : base() { }
        public DomainException(string message) : base(message) { }
    }
}
