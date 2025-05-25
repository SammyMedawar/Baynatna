using System;
using System.Collections.Generic;

namespace Baynatna.ViewModels
{
    public class PostListQuery
    {
        public int? TagId { get; set; }
        public string? Search { get; set; }
        public string SortBy { get; set; } = "date"; // date or likes
        public string SortDir { get; set; } = "desc"; // asc or desc
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 9;
    }
} 