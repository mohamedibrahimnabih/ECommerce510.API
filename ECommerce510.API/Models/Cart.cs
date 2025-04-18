using Microsoft.EntityFrameworkCore;

namespace ECommerce510.API.Models
{
    [PrimaryKey(nameof(ProductId), nameof(ApplicationUserId))]
    public class Cart
    {
        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;

        public string ApplicationUserId { get; set; } = string.Empty;
        public ApplicationUser ApplicationUser { get; set; } = null!;

        public int Count { get; set; }
    }
}
