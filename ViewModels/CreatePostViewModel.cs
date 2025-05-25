using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Baynatna.ViewModels
{
    public class CreatePostViewModel
    {
        [Required]
        [Display(Name = "Post Body")]
        public string OriginalBody { get; set; } = null!;
        public string? TranslatedBody { get; set; }
        public string? VoiceMessageUrl { get; set; }
        public int TagId { get; set; }
    }
} 