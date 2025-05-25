using System.ComponentModel.DataAnnotations;

namespace Baynatna.ViewModels
{
    public class TokenRequestViewModel
    {
        [Required]
        [Phone]
        public string Phone { get; set; } = null!;
        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;
        [Required]
        [Display(Name = "ID or Proof of Residency")]
        public string IdOrProof { get; set; } = null!;
    }
} 