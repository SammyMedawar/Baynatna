using System;

namespace Baynatna.Models
{
    public class CommentVote
    {
        public int Id { get; set; }
        public int CommentId { get; set; }
        public int UserId { get; set; }
        public bool IsUpvote { get; set; }
        public DateTime CreatedAt { get; set; }

        public Comment Comment { get; set; } = null!;
        public User User { get; set; } = null!;
    }
} 