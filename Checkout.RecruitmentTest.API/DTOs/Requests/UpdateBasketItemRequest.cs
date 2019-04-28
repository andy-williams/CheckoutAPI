using System.ComponentModel.DataAnnotations;

namespace Checkout.RecruitmentTest.API.DTOs.Requests
{
    public class UpdateBasketItemRequest
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least {1}")]
        public int Quantity { get; set; }
    }
}