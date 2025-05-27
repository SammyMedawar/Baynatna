using System;

namespace Baynatna.ViewModels
{
    public class ComplaintListQuery
    {
        public string? SearchTerm { get; set; }
        public string? SortBy { get; set; }
        public bool SortDescending { get; set; }
        public int? TagId { get; set; }
        public bool ShowQuarantined { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? Search { get => SearchTerm; set => SearchTerm = value; }
        public string? SortDir { get => SortDescending ? "desc" : "asc"; set => SortDescending = value == "desc"; }
    }
} 