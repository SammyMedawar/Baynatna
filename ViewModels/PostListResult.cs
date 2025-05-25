using System.Collections.Generic;

namespace Baynatna.ViewModels
{
    public class PostListResult
    {
        public List<PostCardViewModel> Posts { get; set; } = new List<PostCardViewModel>();
        public bool HasMore { get; set; }
    }
} 