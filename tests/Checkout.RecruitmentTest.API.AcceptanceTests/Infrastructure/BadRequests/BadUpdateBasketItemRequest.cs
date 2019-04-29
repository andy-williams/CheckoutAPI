using System;
using System.Collections.Generic;
using System.Text;

namespace Checkout.RecruitmentTest.API.AcceptanceTests.Infrastructure.BadRequests
{
    public class BadUpdateBasketItemRequest
    {
        public int? Quantity { get; set; } = null;
    }
}
