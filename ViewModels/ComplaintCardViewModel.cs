namespace Baynatna.ViewModels
{
    public class ComplaintCardViewModel
    {
        public int Id { get; set; }
        public string ThreadId { get; set; } = null!;
        public string OriginalTitle { get; set; } = null!;
        public string? TranslatedTitle { get; set; }
        public int Upvotes { get; set; }
        public int Downvotes { get; set; }
        public int CommentCount { get; set; }
        public DateTime CreatedAt { get; set; }
    }
} 