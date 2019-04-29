namespace Checkout.RecruitmentTest.API.DomainExceptions
{
    public class DuplicateBasketItemException : DomainException
    {
        public DuplicateBasketItemException(string itemRef) : base($"{itemRef} already in basket") { }
    }
}