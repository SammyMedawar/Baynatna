using System;

namespace Baynatna.Models
{
    public class PostVote
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public int UserId { get; set; }
        public bool IsUpvote { get; set; }
        public string Reason { get; set; } = null!;
        public DateTime CreatedAt { get; set; }

        public Post Post { get; set; } = null!;
        public User User { get; set; } = null!;
    }
} 