using Microsoft.EntityFrameworkCore;

namespace ECommerce510.API.Models
{
    [PrimaryKey(nameof(OrderId), nameof(ProductId))]
    public class OrderItem
    {
        public int OrderId { get; set; }
        public Order Order { get; set; } = null!;


        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;

        public int Count { get; set; }
        public double Price { get; set; }
    }
}
