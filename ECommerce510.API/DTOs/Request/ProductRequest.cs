using System.ComponentModel.DataAnnotations;

namespace ECommerce510.API.DTOs.Request
{
    public class ProductRequest
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        [Required]
        public IFormFile File { get; set; } = null!;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public bool Status { get; set; }
        public decimal Discount { get; set; }

        public int CategoryId { get; set; }

        public int BrandId { get; set; }
    }
}
