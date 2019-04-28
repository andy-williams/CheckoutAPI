namespace Checkout.RecruitmentTest.API.AcceptanceTests.Infrastructure.BadRequests
{
    public class BadAddBasketItemRequest
    {
        public string Name { get; set; }
        public string Ref { get; set; }
        public decimal? Price { get; set; }
        public int? Quantity { get; set; }
    }
}
