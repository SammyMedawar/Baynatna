using System.Collections.Generic;
using Baynatna.Models;

namespace Baynatna.ViewModels
{
    public class ComplaintListResult
    {
        public IEnumerable<ComplaintListItemViewModel> Complaints { get; set; } = new List<ComplaintListItemViewModel>();
        public int TotalCount { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public bool HasMore { get; set; }
    }

    public class ComplaintListItemViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Tag { get; set; }
        public int Upvotes { get; set; }
        public int Downvotes { get; set; }
        public int CommentCount { get; set; }
    }
} 