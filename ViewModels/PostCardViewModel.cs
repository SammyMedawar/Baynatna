namespace Baynatna.ViewModels
{
    public class PostCardViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Tag { get; set; } = null!;
        public int Upvotes { get; set; }
        public int Downvotes { get; set; }
    }
} 