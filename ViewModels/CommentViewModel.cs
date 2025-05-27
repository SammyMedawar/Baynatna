namespace Baynatna.ViewModels
{
    public class CommentViewModel
    {
        public int Id { get; set; }
        public int ComplaintId { get; set; }
        public int UserId { get; set; }
        public string Body { get; set; } = null!;
        public string? TranslatedBody { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsOwner { get; set; }
        public int Upvotes { get; set; }
        public int Downvotes { get; set; }
        public bool CanVote { get; set; }
        public bool CanDelete { get; set; }
    }
} 