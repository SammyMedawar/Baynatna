using System.ComponentModel.DataAnnotations;

namespace Baynatna.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        public string Username { get; set; } = null!;
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;
        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; } = null!;
        [Required]
        public string Token { get; set; } = null!;
    }
} 