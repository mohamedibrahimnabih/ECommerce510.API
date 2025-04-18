namespace ECommerce510.API.DTOs.Response
{
    public class CartWithTotalResponse
    {
        public List<CartResponse> Carts { get; set; } = null!;

        public decimal TotalPrice { get; set; }
    }
}
