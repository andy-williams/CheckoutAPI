using System.Collections.Generic;
using Checkout.RecruitmentTest.API.Data;

namespace Checkout.RecruitmentTest.API.DTOs.Responses
{
    public class GetBasketResponse
    {
        public IList<BasketItem> BasketItems { get; set; }
    }
}
