using ECommerce510.API.ValidationAttributes;

namespace ECommerce510.API.DTOs.Request
{
    public class CategoryRequest
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool Status { get; set; }
    }
}
