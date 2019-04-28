using System;

namespace Checkout.RecruitmentTest.API.DTOs.Requests
{
    public class UpdateBasketItemRequest
    {
        public Guid BasketItemId { get; set; }
        public int Quantity { get; set; } = 1;
    }
}