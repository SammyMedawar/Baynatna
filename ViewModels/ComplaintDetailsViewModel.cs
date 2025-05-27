using System;
using System.Collections.Generic;

namespace Baynatna.ViewModels
{
    public class ComplaintDetailsViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Body { get; set; } = null!;
        public string? TranslatedTitle { get; set; }
        public string? TranslatedBody { get; set; }
        public string? VoiceMessageUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsOwner { get; set; }
        public bool? UserVote { get; set; }
        public string? UserVoteReason { get; set; }
        public int Upvotes { get; set; }
        public int Downvotes { get; set; }
        public string? Tag { get; set; }
        public List<CommentViewModel> Comments { get; set; } = new();
    }
} 