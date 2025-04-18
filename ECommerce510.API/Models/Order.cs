namespace ECommerce510.API.Models
{
    public enum OrderStatus
    {
        Pending,
        Canceled,
        InProgress,
        Shipped,
        Completed
    }

    public class Order
    {

        public int OrderId { get; set; }

        public string ApplicationUserId { get; set; } = string.Empty;
        public ApplicationUser ApplicationUser { get; set; } = null!;

        public DateTime OrderDate { get; set; }
        public double OrderTotal { get; set; }
        public OrderStatus Status { get; set; }
        public bool PaymentStatus { get; set; }
        public string? Carrier { get; set; }
        public string? TrackingNumber { get; set; }

        public string? SessionId { get; set; }
        public string? PaymentStripeId { get; set; }

    }
}
