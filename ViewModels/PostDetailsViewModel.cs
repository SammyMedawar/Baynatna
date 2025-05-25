namespace Baynatna.ViewModels
{
    using System.Collections.Generic;

    public class PostDetailsViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string? TranslatedTitle { get; set; }
        public string Body { get; set; } = null!;
        public string? TranslatedBody { get; set; }
        public bool IsTranslatedByModerator { get; set; }
        public string? Tag { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsOwner { get; set; }
        public bool IsQuarantined { get; set; }
        public List<CommentViewModel> Comments { get; set; } = new();
        public AddCommentViewModel NewComment { get; set; } = new();
    }
} 