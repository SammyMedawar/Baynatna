using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Baynatna.ViewModels
{
    public class CreateComplaintViewModel
    {
        [Required]
        public string OriginalTitle { get; set; } = null!;

        [Required]
        public string OriginalBody { get; set; } = null!;

        public string? TranslatedTitle { get; set; }

        public string? TranslatedBody { get; set; }

        public string? VoiceMessageUrl { get; set; }

        public List<int> TagIds { get; set; } = new List<int>();

        public int? TagId { get; set; } // For compatibility with code expecting a single tag
    }
} 