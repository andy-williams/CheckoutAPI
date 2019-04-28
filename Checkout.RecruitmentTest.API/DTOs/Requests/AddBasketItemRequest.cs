using System.ComponentModel.DataAnnotations;

namespace Checkout.RecruitmentTest.API.DTOs.Requests
{
    public class AddBasketItemRequest
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Ref { get; set; }

        [Required]
        [Range(0.01D, double.MaxValue)]
        public decimal Price { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least {1}")]
        public int Quantity { get; set; }
    }
}