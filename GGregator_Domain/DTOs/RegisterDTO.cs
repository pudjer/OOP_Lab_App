using System.ComponentModel.DataAnnotations;

namespace GGregator_Domain.DTOs
{
    public class RegisterDTO
    {
        [Required]
        public string Username { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
        [Required]
        [Compare("Password", ErrorMessage="Password confirmation doesn't match.")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
