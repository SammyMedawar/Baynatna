using System;
using System.Collections.Generic;

namespace Baynatna.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public bool IsModerator { get; set; }
        public DateTime CreatedAt { get; set; }

        public ICollection<Comment>? Comments { get; set; }
        public ICollection<CommentVote>? CommentVotes { get; set; }
        public ICollection<Report>? Reports { get; set; }
        public ICollection<Complaint>? Complaints { get; set; }
        public ICollection<ComplaintVote>? ComplaintVotes { get; set; }
    }
} 