using System.ComponentModel.DataAnnotations;

namespace ECommerce510.API.DTOs.Request
{
    public class RegisterDTO
    {
        [Required]
        public string UserName { get; set; } = string.Empty;
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = string.Empty;
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; } = string.Empty;
        public string? Address { get; set; }
    }
}
