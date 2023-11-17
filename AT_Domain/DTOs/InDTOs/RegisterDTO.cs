using System.ComponentModel.DataAnnotations;

namespace AT_Domain.DTOs.InDTOs
{
    public class RegisterDTO
    {
        [Required]
        public string Username { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
        [Required]
        [Compare("Password", ErrorMessage = "Password confirmation doesn't match.")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
