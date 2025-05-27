using System.ComponentModel.DataAnnotations;

namespace Baynatna.ViewModels
{
    public class AddCommentViewModel
    {
        public int ComplaintId { get; set; }

        [Required(ErrorMessage = "Comment text is required")]
        [MinLength(1, ErrorMessage = "Comment cannot be empty")]
        [Display(Name = "Comment")]
        public string Body { get; set; } = null!;

        public string? TranslatedBody { get; set; }
        
        public string? VoiceMessageUrl { get; set; }

        [Required(ErrorMessage = "Please select upvote or downvote")]
        public string VoteAction { get; set; } = null!;
    }
} 