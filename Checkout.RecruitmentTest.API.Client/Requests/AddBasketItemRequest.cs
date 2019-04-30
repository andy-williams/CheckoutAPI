using System;

namespace Checkout.RecruitmentTest.API.Client.Requests
{
    public class AddBasketItemRequest
    {
        public string Name { get; set; }
        public string Ref { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
