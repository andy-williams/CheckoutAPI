using System;

namespace Checkout.RecruitmentTest.API.Data
{
    public class BasketItem
    {
        public Guid Id { get; set; }

        // item ID given to the item by client
        public string Ref { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}