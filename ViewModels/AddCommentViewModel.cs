using System.ComponentModel.DataAnnotations;

namespace Baynatna.ViewModels
{
    public class AddCommentViewModel
    {
        public int PostId { get; set; }
        [Required]
        [Display(Name = "Comment Body")]
        public string Body { get; set; } = null!;
        public string? TranslatedBody { get; set; }
        public string? VoiceMessageUrl { get; set; }
    }
} 