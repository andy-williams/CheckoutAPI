using System;

namespace Checkout.RecruitmentTest.API.Client.Responses
{
    public class BasketItem
    {
        public Guid Id { get; set; }
        public string Ref { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}