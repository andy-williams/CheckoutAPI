namespace Checkout.RecruitmentTest.API.Client.Internal
{
    internal class AddBasketItemInternalRequest
    {
        public string Name { get; set; }
        public string Ref { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}