using Application.DTO.Base;

namespace Application.DTO
{
    public class EverythingQuery : QueryBase
    {
        public EverythingQuery() { }
        public string? QInTitle { get; set; }
        public string? Domains { get; set; }
        public string? ExcludedDomains { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
        public string? Language { get; set; }
        public string? SortBy { get; set; }
    }
}
